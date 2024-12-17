using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginationResult<UserDTO>> GetAllAsync(PaginationReq paging, CancellationToken ct);
        Task<UserDTO?> GetByIdAsync(long id, CancellationToken ct);
        Task<UserDTO> AddAsync(UserDTO userDTO, CancellationToken ct);
        Task UpdateAsync(UserDTO userDTO, CancellationToken ct);
        Task DeleteAsync(long id, CancellationToken ct);
        Task<bool> ExistsAsync(long id, CancellationToken ct);
    }
}
