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

namespace SkyEagle.Controllers;

[Route("api/product")]
[ApiController]
public class ProductsController(IProductRepository productRepository, IHttpContextAccessor contextAccessor) : BaseController(contextAccessor)
{
	private readonly IProductRepository _productRepository = productRepository;

	[HttpGet]
	public async Task<IActionResult> GetProducts([FromQuery] PaginationReq paging, CancellationToken ct = default)
	{
		paging.CheckValidate();
		PaginationResult<ProductGridItemDTO> result = await _productRepository.GetAllAsync(paging, ct);
		return Ok(result);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ProductResponseDTO>> GetProduct(long id, CancellationToken ct = default)
	{
		ProductResponseDTO? product = await _productRepository.GetByIdAsync(id, ct);
		if (product == null)
			return NotFound();
		return product;
	}

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
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

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

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(long id, CancellationToken ct = default)
	{
		await _productRepository.DeleteAsync(id, ct);
		return NoContent();
	}
}