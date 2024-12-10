using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using SkyEagle.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Controllers
{
	[Route("api/product")]
	[ApiController]
	public class ProductsController(IProductRepository productRepository, IHttpContextAccessor contextAccessor) : BaseController(contextAccessor)
	{
		private readonly IProductRepository _productRepository = productRepository;

		// GET: api/product
		[HttpGet]
		public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, CancellationToken ct = default)
		{
			if (pageNumber <= 0 || pageSize <= 0)
				return BadRequest("Số trang và size trang phải lớn hơn 0.");
			PaginationResult<ProductGridDTO> result = await _productRepository.GetAllAsync(pageNumber, pageSize, search, ct);
			return Ok(result);
		}

		// GET: api/product/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductResponseDTO>> GetProduct(long id, CancellationToken ct = default)
		{
			ProductResponseDTO? product = await _productRepository.GetByIdAsync(id, ct);
			if (product == null)
				return NotFound();
			return product;
		}

		// PUT: api/product/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(long id, ProductDTO productDTO, CancellationToken ct = default)
		{
			if (id != productDTO.Id)
				return BadRequest();

			try
			{
				await _productRepository.UpdateAsync(productDTO, ct);
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await _productRepository.ExistsAsync(id, ct))
					return NotFound();
				throw;
			}
		}

		// POST: api/product
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDTO, CancellationToken ct = default)
		{
			try
			{
				ProductDTO createdProduct = await _productRepository.AddAsync(productDTO, ct);
				return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// DELETE: api/product/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(long id, CancellationToken ct = default)
		{
			await _productRepository.DeleteAsync(id, ct);
			return NoContent();
		}
	}
}
