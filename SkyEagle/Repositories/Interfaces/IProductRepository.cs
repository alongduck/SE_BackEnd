﻿using SkyDTO;
using SkyDTO.Commons;
using SkyEagle.Classes;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEagle.Repositories.Interfaces;

public interface IProductRepository
{
	Task<ProductResponseDTO?> GetByIdAsync(long id, CancellationToken ct = default);

	Task<PaginationResult<ProductGridItemDTO>> GetAllAsync(PaginationReq paging, CancellationToken ct = default);

	Task<ProductDTO> AddAsync(ProductDTO productDTO, CancellationToken ct = default);

	Task UpdateAsync(ProductDTO productDTO, CancellationToken ct = default);

	Task DeleteAsync(long id, CancellationToken ct = default);

	Task<bool> ExistsAsync(long id, CancellationToken ct = default);
}