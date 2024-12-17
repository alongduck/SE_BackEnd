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

[Route("api/category")]
[ApiController]
public class CategoriesController(ICategoryRepository categoryRepository, IHttpContextAccessor contextAccessor) : BaseController(contextAccessor)
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    // GET: api/category
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] PaginationReq paging, CancellationToken ct = default)
    {
        paging.CheckValidate();
        PaginationResult<CategoryDTO> result = await _categoryRepository.GetAllAsync(paging, ct);
        return Ok(result);
    }

    // GET: api/category/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategory(long id, CancellationToken ct = default)
    {
        CategoryDTO? category = await _categoryRepository.GetByIdAsync(id, ct);
        if (category == null)
            return NotFound("Danh mục không tồn tại.");
        return Ok(category);
    }

    // POST: api/category
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> PostCategory([FromBody] CategoryDTO categoryDTO, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            CategoryDTO createdCategory = await _categoryRepository.AddAsync(categoryDTO, ct);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi tạo danh mục: {ex.Message}");
        }
    }

    // PUT: api/category/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(long id, [FromBody] CategoryDTO categoryDTO, CancellationToken ct = default)
    {
        if (id != categoryDTO.Id)
            return BadRequest("ID không khớp.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _categoryRepository.UpdateAsync(categoryDTO, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Danh mục không tồn tại.");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _categoryRepository.ExistsAsync(id, ct))
                return NotFound("Danh mục không tồn tại.");
            throw;
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi cập nhật danh mục: {ex.Message}");
        }
    }

    // DELETE: api/category/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(long id, CancellationToken ct = default)
    {
        try
        {
            await _categoryRepository.DeleteAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Danh mục không tồn tại.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi xóa danh mục: {ex.Message}");
        }
    }
}
