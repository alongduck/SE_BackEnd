using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PaginationResult<CategoryDTO>> GetAllAsync(PaginationReq paging, CancellationToken ct);
        Task<CategoryDTO?> GetByIdAsync(long id, CancellationToken ct);
        Task<CategoryDTO> AddAsync(CategoryDTO categoryDTO, CancellationToken ct);
        Task UpdateAsync(CategoryDTO categoryDTO, CancellationToken ct);
        Task DeleteAsync(long id, CancellationToken ct);
        Task<bool> ExistsAsync(long id, CancellationToken ct);
    }
}
