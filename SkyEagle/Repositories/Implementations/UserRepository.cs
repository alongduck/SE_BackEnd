using Microsoft.EntityFrameworkCore;
using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Repositories.Interfaces;
using SkyModel.Models;
using SkyModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SkyEagle.Classes;
using System.Collections.Generic;

namespace SkyEagle.Repositories
{
    public class UserRepository(SkyDbContext context) : IUserRepository
    {
        private readonly SkyDbContext _context = context;

        public async Task<PaginationResult<UserDTO>> GetAllAsync(PaginationReq paging, CancellationToken ct)
        {
            var query = _context.Users.AsQueryable();
            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(u => u.Name)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                    Avatar = u.Avatar,
                    IsActive = u.IsActive
                })
                .ToListAsync(ct);

            return new PaginationResult<UserDTO>(items, total, paging.PageNumber, paging.PageSize);
        }

        public async Task<UserDTO?> GetByIdAsync(long id, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(new object[] { id }, ct);
            return user == null ? null : new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Avatar = user.Avatar,
                IsActive = user.IsActive
            };
        }

        public async Task<UserDTO> AddAsync(UserDTO userDTO, CancellationToken ct)
        {
            var user = new User
            {
                Name = userDTO.Name,
                PhoneNumber = userDTO.PhoneNumber,
                Avatar = userDTO.Avatar,
                IsActive = userDTO.IsActive
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(ct);

            userDTO.Id = user.Id;
            return userDTO;
        }

        public async Task UpdateAsync(UserDTO userDTO, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(new object[] { userDTO.Id }, ct)
                ?? throw new KeyNotFoundException("User không tồn tại");

            user.Name = userDTO.Name;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Avatar = userDTO.Avatar;
            user.IsActive = userDTO.IsActive;

            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(long id, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(new object[] { id }, ct)
                ?? throw new KeyNotFoundException("User không tồn tại");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(long id, CancellationToken ct)
        {
            return await _context.Users.AnyAsync(u => u.Id == id, ct);
        }
    }
}
