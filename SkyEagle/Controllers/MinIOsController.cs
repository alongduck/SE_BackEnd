using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SkyDTO.Commons;
using SkyEagle.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Controllers;

[Route("api/minio")]
[ApiController]
public class MinIOsController(IMinIORepository minIORepository, IHttpContextAccessor contextAccessor) : BaseController(contextAccessor)
{
	private readonly IMinIORepository _minIORepository = minIORepository;

	[HttpGet("/minio/file/{id}")]
	public async Task<IActionResult> GetFile(long id, CancellationToken ct = default)
	{
		KeyValuePair<string, byte[]>? minIOFile = await _minIORepository.GetFileAsync(id, ct);
		if (!minIOFile.HasValue)
			return NotFound();
		if (httpContextAccessor.HttpContext != null && !httpContextAccessor.HttpContext.Response.HasStarted && !httpContextAccessor.HttpContext.Request.Headers.IsReadOnly)
			try
			{
				httpContextAccessor.HttpContext.Response.Headers[HeaderNames.CacheControl] = "public,max-age=31536000";
			}
			catch { }
		return File(minIOFile.Value.Value, MediaTypeNames.Application.Octet, minIOFile.Value.Key);
	}

	// POST:api/minio/image
	[HttpPost("image")]
	public async Task<IActionResult> UploadImage(IFormFile image, CancellationToken ct = default)
	{
		try
		{
			ImageDTO createdImage = await _minIORepository.UploadImageAsync(image, ct);
			return CreatedAtAction(nameof(GetFile), new { id = createdImage.Id }, createdImage);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	// DELETE: api/minio/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteFile(long id, CancellationToken ct = default)
	{
		await _minIORepository.DeleteFileAsync(id, ct);
		return NoContent();
	}
}