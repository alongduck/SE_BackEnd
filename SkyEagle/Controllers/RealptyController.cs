using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyDTO;
using System.Threading.Tasks;

namespace SkyEagle.Controllers;

[Route("api/realpty")]
public class RealptyController : BaseController
{
	public RealptyController(IHttpContextAccessor contextAccessor) : base(contextAccessor) { }

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		await Task.CompletedTask;
		return Ok();
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] RealptyDTO realpty)
	{
		await Task.CompletedTask;
		return Ok(realpty.Id);
	}
}