using SkyDTO;
using SkyDTO.Commons;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Interfaces
{
	public interface IProductRepository
	{
		Task<ProductDTO?> GetByIdAsync(long id, CancellationToken ct = default);
		Task<IEnumerable<ProductGridDTO>> GetAllAsync(CancellationToken ct = default);
		Task<ProductDTO> AddAsync(ProductDTO productDTO, CancellationToken ct = default);
		Task UpdateAsync(ProductDTO productDTO, CancellationToken ct = default);
		Task DeleteAsync(long id, CancellationToken ct = default);
		Task<bool> ExistsAsync(long id, CancellationToken ct = default);
	}
}
