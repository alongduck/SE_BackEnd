using System;
using System.Collections.Generic;

namespace SkyEagle.Classes;

public class PaginationResult<T>(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
{
	public int TotalCount { get; set; } = totalCount;
	public int PageNumber { get; set; } = pageNumber;
	public int PageSize { get; set; } = pageSize;
	public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
	public IEnumerable<T> Items { get; set; } = items;
}