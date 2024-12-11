using System;
using System.ComponentModel.DataAnnotations;

namespace SkyDTO.Commons
{
	public class ProductResponseDTO
	{
		public long Id { get; set; }

		[Display(Name = "Tên")]
		public string Name { get; set; } = string.Empty;

		[Display(Name = "Thumbnail")]
		public ImageDTO? Thumbnail { get; set; }

		[Display(Name = "Giá")]
		public double Price { get; set; }

		[Display(Name = "Hot")]
		public bool Hot { get; set; }

		[Display(Name = "Ngày đăng")]
		public DateTime TimeUp { get; set; } = DateTime.Now;

		[Display(Name = "Danh mục")]
		public CategoryDTO? Category { get; set; }

		[Display(Name = "Người dùng")]
		public UserDTO? User { get; set; }

		[Display(Name = "Chi tiết")]
		public ProductDetailDTO? Detail { get; set; }
	}
}
