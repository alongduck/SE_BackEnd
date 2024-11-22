using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace SkyEagle.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
	protected readonly IHttpContextAccessor httpContextAccessor;

	public BaseController(IHttpContextAccessor contextAccessor)
	{
		httpContextAccessor = contextAccessor;
	}

	protected StringValues GetHeader(string key)
	{
		if (httpContextAccessor.HttpContext == null || !httpContextAccessor.HttpContext.Response.Headers.TryGetValue(key, out StringValues value))
			return string.Empty;
		return value;
	}
}