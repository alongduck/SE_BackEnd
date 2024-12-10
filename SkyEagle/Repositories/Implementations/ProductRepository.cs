using Microsoft.EntityFrameworkCore;
using SkyDTO;
using SkyEagle.Repositories.Interfaces;
using SkyModel;
using SkyModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Implementations
{
	public class ProductRepository(SkyDbContext context) : IProductRepository
	{
		private readonly SkyDbContext _context = context;

		public async Task<ProductDTO?> GetByIdAsync(long id, CancellationToken ct = default)
		{
			Product? product = await _context.Products.FindAsync([id], cancellationToken: ct);
			if (product == null)
				return null;
			return ProductToDTO(product);
		}

		public async Task<IEnumerable<ProductDTO>> GetAllAsync(CancellationToken ct = default)
		{
			return await _context.Products.Select(x => ProductToDTO(x, false)).ToListAsync(ct);
		}

		public async Task<ProductDTO> AddAsync(ProductDTO productDTO, CancellationToken ct = default)
		{
			// Validate khóa ngoại tồn tại
			bool categoryExists = await _context.Categories.AnyAsync(x => x.Id == productDTO.CategoryId, ct);
			if (!categoryExists)
				throw new ArgumentException("ID danh mục không tồn tại");
			bool userExists = await _context.Users.AnyAsync(x => x.Id == productDTO.UserId, ct);
			if (!userExists)
				throw new ArgumentException("ID người dùng không tồn tại");

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
				}
				else
				{
					// Tạo mới detail
					productDetail = new();
				}

				_context.ProductDetails.Add(productDetail);
				await _context.SaveChangesAsync(ct);

				// Tạo sản phẩm mới
				Product product = new()
				{
					Name = productDTO.Name,
					Thumbnail = productDTO.Thumbnail,
					Price = productDTO.Price,
					Hot = productDTO.Hot,
					TimeUp = productDTO.TimeUp,
					CategoryId = productDTO.CategoryId,
					UserId = productDTO.UserId,
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
			Product product = await _context.Products.FindAsync([productDTO.Id], cancellationToken: ct)
				?? throw new KeyNotFoundException($"Không tìm thấy sản phẩm");
			product.Name = productDTO.Name;
			product.Thumbnail = productDTO.Thumbnail;
			product.Price = productDTO.Price;
			product.Hot = productDTO.Hot;
			product.TimeUp = productDTO.TimeUp;
			product.CategoryId = productDTO.CategoryId;
			product.UserId = productDTO.UserId;
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
			}
			await _context.SaveChangesAsync(ct);
		}

		public async Task DeleteAsync(long id, CancellationToken ct = default)
		{
			Product? product = await _context.Products.Include(p => p.ObjDetail).FirstOrDefaultAsync(x => x.Id == id, ct);
			if (product != null)
			{
				if (product.ObjDetail != null)
					_context.ProductDetails.Remove(product.ObjDetail);
				_context.Products.Remove(product);
				await _context.SaveChangesAsync(ct);
			}
		}

		public async Task<bool> ExistsAsync(long id, CancellationToken ct = default)
		{
			return await _context.Products.AnyAsync(e => e.Id == id, ct);
		}

		private static ProductDTO ProductToDTO(Product product, bool loadDetail = true) =>
		   new()
		   {
			   Id = product.Id,
			   Name = product.Name,
			   Thumbnail = product.Thumbnail,
			   Price = product.Price,
			   Hot = product.Hot,
			   TimeUp = product.TimeUp,
			   CategoryId = product.CategoryId,
			   UserId = product.UserId,
			   Detail = loadDetail ? ProductDetailToDTO(product.ObjDetail) : null
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
				Structure = productDetail.Structure
			};
	}
}
