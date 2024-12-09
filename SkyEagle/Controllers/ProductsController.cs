using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyDTO;
using SkyModel;
using SkyModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Controllers
{
	[Route("api/product")]
	[ApiController]
	public class ProductsController(SkyDbContext context, IHttpContextAccessor contextAccessor) : BaseController(contextAccessor)
	{
		private readonly SkyDbContext _context = context;

		// GET: api/product
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(CancellationToken ct = default)
		{
			return await _context.Products.Select(x => new ProductDTO
			{
				Id = x.Id,
				Name = x.Name,
				Thumbnail = x.Thumbnail,
				Price = x.Price,
				Hot = x.Hot,
				TimeUp = x.TimeUp
			}).ToListAsync(ct);
		}

		// GET: api/product/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(long id, CancellationToken ct = default)
		{
			Product? product = await _context.Products.FindAsync([id], cancellationToken: ct);
			if (product == null)
				return NotFound();
			return product;
		}

		// PUT: api/product/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(long id, Product productDTO, CancellationToken ct = default)
		{
			if (id != productDTO.Id)
				return BadRequest();
			Product? product = await _context.Products.FindAsync([id], cancellationToken: ct);
			if (product == null)
				return NotFound();
			product.Name = productDTO.Name;
			product.Thumbnail = productDTO.Thumbnail;
			product.Price = productDTO.Price;
			product.Hot = productDTO.Hot;
			product.TimeUp = productDTO.TimeUp;
			product.CategoryId = productDTO.CategoryId;
			product.DetailId = productDTO.DetailId;
			product.UserId = productDTO.UserId;
			try
			{
				await _context.SaveChangesAsync(ct);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await ProductExistsAsync(id, ct))
					return NotFound();
				else
					throw;
			}
			return NoContent();
		}

		// POST: api/product
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDTO, CancellationToken ct = default)
		{
			Product product = new()
			{
				Name = productDTO.Name,
				Thumbnail = productDTO.Thumbnail,
				Price = productDTO.Price,
				Hot = productDTO.Hot,
				TimeUp = productDTO.TimeUp,
				CategoryId = productDTO.CategoryId,
				DetailId = productDTO.DetailId,
				UserId = productDTO.UserId
			};
			_context.Products.Add(product);
			await _context.SaveChangesAsync(ct);
			return CreatedAtAction("GetProduct", new { id = product.Id }, product);
		}

		// DELETE: api/products/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(long id, CancellationToken ct = default)
		{
			Product? product = await _context.Products.FindAsync([id], cancellationToken: ct);
			if (product == null)
				return NotFound();
			_context.Products.Remove(product);
			await _context.SaveChangesAsync(ct);
			return NoContent();
		}

		private async Task<bool> ProductExistsAsync(long id, CancellationToken ct = default)
		{
			return await _context.Products.AnyAsync(e => e.Id == id, ct);
		}
	}
}
