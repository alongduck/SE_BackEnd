using Microsoft.EntityFrameworkCore;
using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using SkyEagle.Repositories.Interfaces;
using SkyModel.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SkyModel;

namespace SkyEagle.Repositories
{
    public class CategoryRepository(SkyDbContext context) : ICategoryRepository
    {
        private readonly SkyDbContext _context = context;

        public async Task<PaginationResult<CategoryDTO>> GetAllAsync(PaginationReq paging, CancellationToken ct)
        {
            var query = _context.Categories.AsQueryable();
            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(c => c.Name)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .Select(c => new CategoryDTO { Id = c.Id, Name = c.Name })
                .ToListAsync(ct);

            return new PaginationResult<CategoryDTO>(
                items: items,              // Danh sách dữ liệu
                totalCount: total,         // Tổng số bản ghi
                pageNumber: paging.PageNumber, // Trang hiện tại
                pageSize: paging.PageSize      // Số lượng bản ghi trên mỗi trang
            );
        }


        public async Task<CategoryDTO?> GetByIdAsync(long id, CancellationToken ct)
        {
            var category = await _context.Categories.FindAsync(new object[] { id }, ct);
            return category == null ? null : new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task<CategoryDTO> AddAsync(CategoryDTO categoryDTO, CancellationToken ct)
        {
            var category = new Category { Name = categoryDTO.Name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(ct);
            categoryDTO.Id = category.Id;
            return categoryDTO;
        }

        public async Task UpdateAsync(CategoryDTO categoryDTO, CancellationToken ct)
        {
            var category = await _context.Categories.FindAsync(new object[] { categoryDTO.Id }, ct)
                ?? throw new KeyNotFoundException();

            category.Name = categoryDTO.Name;
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(long id, CancellationToken ct)
        {
            var category = await _context.Categories.FindAsync(new object[] { id }, ct)
                ?? throw new KeyNotFoundException();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(long id, CancellationToken ct)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id, ct);
        }
    }
}
