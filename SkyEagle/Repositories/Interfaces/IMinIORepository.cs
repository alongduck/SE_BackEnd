using Microsoft.AspNetCore.Http;
using SkyDTO.Commons;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Interfaces
{
	public interface IMinIORepository
	{
		Task<KeyValuePair<string, byte[]>?> GetFileAsync(long id, CancellationToken ct = default);
		Task<ImageDTO> UploadImageAsync(IFormFile image, CancellationToken ct = default);
		Task DeleteFileAsync(long id, CancellationToken ct = default);
	}
}
