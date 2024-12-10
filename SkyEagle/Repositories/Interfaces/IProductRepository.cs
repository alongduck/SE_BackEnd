using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Interfaces
{
	public interface IProductRepository
	{
		Task<ProductDTO?> GetByIdAsync(long id, CancellationToken ct = default);
		Task<PaginationResult<ProductGridDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
		Task<ProductDTO> AddAsync(ProductDTO productDTO, CancellationToken ct = default);
		Task UpdateAsync(ProductDTO productDTO, CancellationToken ct = default);
		Task DeleteAsync(long id, CancellationToken ct = default);
		Task<bool> ExistsAsync(long id, CancellationToken ct = default);
	}
}
