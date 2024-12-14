namespace SkyEagle.Classes;

internal static class ExtensionMethods
{
	internal static void CheckValidate(this PaginationReq paging)
	{
		if (paging.PageNumber <= 0)
			paging.PageNumber = 1;
		if (paging.PageSize <= 0 || paging.PageSize > 100)
			paging.PageSize = 100; // mức tối đa
	}
}