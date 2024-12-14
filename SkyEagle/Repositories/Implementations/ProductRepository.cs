using Microsoft.EntityFrameworkCore;
using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using SkyEagle.Repositories.Interfaces;
using SkyModel;
using SkyModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Implementations;

public class ProductRepository(SkyDbContext context) : IProductRepository
{
	private readonly SkyDbContext _context = context;

	public async Task<ProductResponseDTO?> GetByIdAsync(long id, CancellationToken ct = default)
	{
		Product? product = await _context.Products.FindAsync([id], cancellationToken: ct);
		if (product == null)
			return null;
		return ProductToResponseDTO(product);
	}

	public async Task<PaginationResult<ProductGridItemDTO>> GetAllAsync(PaginationReq paging, CancellationToken ct = default)
	{
		IQueryable<Product> query = _context.Products.AsNoTracking();

		// Search nếu có key
		if (!string.IsNullOrWhiteSpace(paging.Search))
			query = query.Where(product => EF.Functions.Like(product.Name, $"%{paging.Search}%"));

		int totalCount = await query.CountAsync(ct);
		List<ProductGridItemDTO> products = await query
			.OrderByDescending(product => product.Id)
			.Skip((paging.PageNumber - 1) * paging.PageSize)
			.Take(paging.PageSize)
			.Select(product => new ProductGridItemDTO
			{
				Id = product.Id,
				Name = product.Name,
				Thumbnail = product.ObjThumbnailImage != null
				   ? new()
				   {
					   Id = product.ObjThumbnailImage.Id,
					   Src = CommonFunctions.GetMinIODisplayPath(product.ObjThumbnailImage.Id),
					   Alt = "Ảnh thumbnail"
				   }
				   : null,
				Price = product.Price,
				Hot = product.Hot,
				Area = product.ObjDetail != null ? product.ObjDetail.Area : null,
				TimeUp = product.TimeUp,
				Category = new()
				{
					Id = product.ObjCategory.Id,
					Name = product.ObjCategory.Name
				}
			})
			.ToListAsync(ct);
		return new PaginationResult<ProductGridItemDTO>(products, totalCount, paging.PageNumber, paging.PageSize);
	}

	public async Task<ProductDTO> AddAsync(ProductDTO productDTO, CancellationToken ct = default)
	{
		await ValidateProductAsync(productDTO, ct);
		using var transaction = await _context.Database.BeginTransactionAsync(ct);
		try
		{
			// Tạo chi tiết sản phẩm
			ProductDetail productDetail;
			if (productDTO.Detail != null)
			{
				// Add detail từ api
				ProductDetailDTO productDetailDTO = productDTO.Detail;
				productDetail = new()
				{
					Description = productDetailDTO.Description,
					Address = productDetailDTO.Address,
					PricePerSquareMeter = productDetailDTO.PricePerSquareMeter,
					Features = productDetailDTO.Features,
					Area = productDetailDTO.Area,
					Length = productDetailDTO.Length,
					Width = productDetailDTO.Width,
					Structure = productDetailDTO.Structure
				};
				_context.ProductDetails.Add(productDetail);
				await _context.SaveChangesAsync(ct);

				// Check nếu có hình thì update khóa ngoại detail vào
				if (productDetailDTO.Images?.Count > 0)
				{
					List<long> imageIds = productDetailDTO.Images.Select(x => x.Id).ToList();
					await _context.MinIOs
						.Where(x => imageIds.Contains(x.Id))
						.ExecuteUpdateAsync(s => s.SetProperty(m => m.ProductDetailId, productDetail.Id), ct);

					// Load hình
					productDetail.Images = await _context.MinIOs
						.Where(x => x.ProductDetailId == productDetail.Id)
						.ToListAsync(ct);
				}
			}
			else
			{
				// Tạo mới detail
				productDetail = new();
				_context.ProductDetails.Add(productDetail);
				await _context.SaveChangesAsync(ct);
			}

			// Tạo sản phẩm mới
			Product product = new()
			{
				Name = productDTO.Name,
				Price = productDTO.Price,
				Hot = productDTO.Hot,
				TimeUp = productDTO.TimeUp,
				CategoryId = productDTO.CategoryId,
				UserId = productDTO.UserId,
				ThumbnailImageId = productDTO.ThumbnailImageId,
				DetailId = productDetail.Id
			};
			_context.Products.Add(product);
			await _context.SaveChangesAsync(ct);

			// Commit transaction
			await transaction.CommitAsync(ct);

			return ProductToDTO(product);
		}
		catch
		{
			// Rollback transaction on failure
			await transaction.RollbackAsync(ct);
			throw;
		}
	}

	public async Task UpdateAsync(ProductDTO productDTO, CancellationToken ct = default)
	{
		await ValidateProductAsync(productDTO, ct);
		using var transaction = await _context.Database.BeginTransactionAsync(ct);
		try
		{
			Product product = await _context.Products.Include(x => x.ObjThumbnailImage).Include(x => x.ObjDetail).FirstOrDefaultAsync(x => x.Id == productDTO.Id, cancellationToken: ct)
				?? throw new KeyNotFoundException($"Không tìm thấy sản phẩm");
			product.Name = productDTO.Name;
			product.Price = productDTO.Price;
			product.Hot = productDTO.Hot;
			product.TimeUp = productDTO.TimeUp;
			product.CategoryId = productDTO.CategoryId;
			product.UserId = productDTO.UserId;

			// Check nếu có ThumbnailImageId
			if (productDTO.ThumbnailImageId != null)
			{
				// Xóa ảnh cũ nếu id mới khác cũdiffers
				if (product.ThumbnailImageId != productDTO.ThumbnailImageId)
				{
					if (product.ObjThumbnailImage != null)
					{
						await RemoveMinIOFileAsync(product.ObjThumbnailImage.FileName, ct);
						product.ThumbnailImageId = null; // Set về null để delete MinIO
						_context.MinIOs.Remove(product.ObjThumbnailImage);
					}
				}

				// Update hình mới
				product.ThumbnailImageId = productDTO.ThumbnailImageId;
			}
			else
			{
				// Nếu không có hình mới thì xóa hình cũ
				if (product.ThumbnailImageId != null && product.ObjThumbnailImage != null)
				{
					await RemoveMinIOFileAsync(product.ObjThumbnailImage.FileName, ct);
					product.ThumbnailImageId = null; // Set về null để delete MinIO
					_context.MinIOs.Remove(product.ObjThumbnailImage);
				}

				// Set hình về null
				product.ThumbnailImageId = null;
			}

			if (productDTO.Detail != null)
			{
				// Update detail từ api
				ProductDetailDTO productDetailDTO = productDTO.Detail;
				product.ObjDetail.Description = productDetailDTO.Description;
				product.ObjDetail.Address = productDetailDTO.Address;
				product.ObjDetail.PricePerSquareMeter = productDetailDTO.PricePerSquareMeter;
				product.ObjDetail.Features = productDetailDTO.Features;
				product.ObjDetail.Area = productDetailDTO.Area;
				product.ObjDetail.Length = productDetailDTO.Length;
				product.ObjDetail.Width = productDetailDTO.Width;
				product.ObjDetail.Structure = productDetailDTO.Structure;

				if (productDetailDTO.Images?.Count > 0)
				{
					List<long> imageIds = productDetailDTO.Images.Select(x => x.Id).ToList();

					// Remove những hình hiện tại trong db và minIO
					List<MinIO> removedMinIOs = await _context.MinIOs
						.Where(x => x.ProductDetailId == product.ObjDetail.Id && !imageIds.Contains(x.Id))
						.ToListAsync(ct);
					if (removedMinIOs.Count > 0)
					{
						List<string> removedOnlineFilePaths = removedMinIOs.Select(x => x.FileName).ToList();
						await RemoveMinIOFilesAsync(removedOnlineFilePaths, ct);
						_context.MinIOs.RemoveRange(removedMinIOs);
					}

					// Update khóa ngoại những hình mới vào
					await _context.MinIOs
						.Where(x => imageIds.Contains(x.Id))
						.ExecuteUpdateAsync(s => s.SetProperty(m => m.ProductDetailId, product.ObjDetail.Id), ct);
				}
				else
				{
					// Không có hình thì remove hết khóa ngoại hình đi
					List<MinIO> removedMinIOs = await _context.MinIOs
						.Where(x => x.ProductDetailId == product.ObjDetail.Id)
						.ToListAsync(ct);
					if (removedMinIOs.Count > 0)
					{
						List<string> removedOnlineFilePaths = removedMinIOs.Select(x => x.FileName).ToList();
						await RemoveMinIOFilesAsync(removedOnlineFilePaths, ct);
						_context.MinIOs.RemoveRange(removedMinIOs);
					}
				}
			}
			await _context.SaveChangesAsync(ct);
			await transaction.CommitAsync(ct);
		}
		catch
		{
			// Rollback the transaction
			await transaction.RollbackAsync(ct);
			throw;
		}
	}

	public async Task DeleteAsync(long id, CancellationToken ct = default)
	{
		Product? product = await _context.Products.Include(p => p.ObjDetail).ThenInclude(d => d.Images).Include(p => p.ObjThumbnailImage).FirstOrDefaultAsync(x => x.Id == id, ct);
		if (product != null)
		{
			// Xóa ảnh trong chi tiết sản phẩm
			if (product.ObjDetail?.Images?.Count > 0)
			{
				List<string> removedDetailImagePaths = product.ObjDetail.Images.Select(x => x.FileName).ToList();
				await RemoveMinIOFilesAsync(removedDetailImagePaths, ct);
				_context.MinIOs.RemoveRange(product.ObjDetail.Images);
			}

			// Xóa ảnh thumbnail
			if (product.ObjThumbnailImage != null)
			{
				await RemoveMinIOFileAsync(product.ObjThumbnailImage.FileName, ct);
				_context.MinIOs.Remove(product.ObjThumbnailImage);
			}

			// Xóa chi tiết sản phẩm
			if (product.ObjDetail != null)
				_context.ProductDetails.Remove(product.ObjDetail);

			// Xóa sản phẩm
			_context.Products.Remove(product);

			// Save
			await _context.SaveChangesAsync(ct);
		}
	}

	private async Task ValidateProductAsync(ProductDTO productDTO, CancellationToken ct)
	{
		// Check danh mục tồn tại
		if (!await _context.Categories.AsNoTracking().AnyAsync(x => x.Id == productDTO.CategoryId, ct))
			throw new ArgumentException("ID danh mục không tồn tại");

		// Check user tồn tại
		if (!await _context.Users.AsNoTracking().AnyAsync(x => x.Id == productDTO.UserId, ct))
			throw new ArgumentException("ID người dùng không tồn tại");

		// Check thumbnail tồn tại
		if (productDTO.ThumbnailImageId.HasValue && !await _context.MinIOs.AsNoTracking().AnyAsync(x => x.Id == productDTO.ThumbnailImageId, ct))
			throw new ArgumentException("ID thumbnail không tồn tại");
	}

	public async Task<bool> ExistsAsync(long id, CancellationToken ct = default)
	{
		return await _context.Products.AsNoTracking().AnyAsync(e => e.Id == id, ct);
	}

	private static async Task RemoveMinIOFileAsync(string removedOnlineFilePath, CancellationToken ct = default)
	{
		using MyMinIO myMinIO = MinIORepository.CreateMinIOInstance();
		await myMinIO.RemoveFileAsync(removedOnlineFilePath, ct);
	}

	private static async Task RemoveMinIOFilesAsync(List<string> removedOnlineFilePaths, CancellationToken ct = default)
	{
		using MyMinIO myMinIO = MinIORepository.CreateMinIOInstance();
		await myMinIO.RemoveFilesAsync(removedOnlineFilePaths, ct);
	}

	private static ProductResponseDTO ProductToResponseDTO(Product product) =>
	   new()
	   {
		   Id = product.Id,
		   Name = product.Name,
		   Thumbnail = product.ObjThumbnailImage != null
			   ? new()
			   {
				   Id = product.ObjThumbnailImage.Id,
				   Src = CommonFunctions.GetMinIODisplayPath(product.ObjThumbnailImage.Id),
				   Alt = "Ảnh thumbnail"
			   }
			   : null,
		   Price = product.Price,
		   Hot = product.Hot,
		   TimeUp = product.TimeUp,
		   Category = new()
		   {
			   Id = product.ObjCategory.Id,
			   Name = product.ObjCategory.Name,
		   },
		   User = new()
		   {
			   Id = product.UserId!.Value,
			   Name = product.ObjUser.Name,
			   PhoneNumber = product.ObjUser.PhoneNumber,
			   Avatar = product.ObjUser.Avatar,
			   IsActive = product.ObjUser.IsActive
		   },
		   Detail = ProductDetailToDTO(product.ObjDetail)
	   };

	private static ProductDTO ProductToDTO(Product product) =>
		new()
		{
			Id = product.Id,
			Name = product.Name,
			ThumbnailImageId = product.ThumbnailImageId,
			Price = product.Price,
			Hot = product.Hot,
			TimeUp = product.TimeUp,
			CategoryId = product.CategoryId,
			UserId = product.UserId,
			Detail = ProductDetailToDTO(product.ObjDetail)
		};

	private static ProductDetailDTO ProductDetailToDTO(ProductDetail productDetail) =>
		new()
		{
			Id = productDetail.Id,
			Description = productDetail.Description,
			Address = productDetail.Address,
			PricePerSquareMeter = productDetail.PricePerSquareMeter,
			Features = productDetail.Features,
			Area = productDetail.Area,
			Length = productDetail.Length,
			Width = productDetail.Width,
			Structure = productDetail.Structure,
			Images = productDetail.Images.Select((x, index) => new ImageDTO
			{
				Id = x.Id,
				Src = CommonFunctions.GetMinIODisplayPath(x.Id),
				Alt = $"Ảnh {index + 1}"
			}).ToList()
		};
}