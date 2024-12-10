using System;
using System.ComponentModel.DataAnnotations;

namespace SkyDTO
{
	public class ProductDTO
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tên")]
		public string Name { get; set; } = string.Empty;

		[Display(Name = "Thumbnail")]
		public string? Thumbnail { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Giá")]
		public double Price { get; set; }

		[Display(Name = "Hot")]
		public bool Hot { get; set; }

		[Display(Name = "Ngày đăng")]
		public DateTime TimeUp { get; set; } = DateTime.Now;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Danh mục")]
		public long? CategoryId { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Người đăng")]
		public long? UserId { get; set; }
	}
}
