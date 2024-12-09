using System.ComponentModel.DataAnnotations;

namespace SkyDTO
{
	public class CategoryDTO
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tên")]
		public string Name { get; set; } = string.Empty;
	}
}
