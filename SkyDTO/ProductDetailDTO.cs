using System.ComponentModel.DataAnnotations;

namespace SkyDTO
{
	public class ProductDetailDTO
	{
		public long Id { get; set; }

		[Display(Name = "Mô tả")]
		public string? Description { get; set; }

		[Display(Name = "Địa chỉ")]
		public string? Address { get; set; }

		[Display(Name = "Giá/m2")]
		public double? PricePerSquareMeter { get; set; }

		[Display(Name = "Tính năng")]
		public string? Features { get; set; }

		[Display(Name = "Diện tích")]
		public double? Area { get; set; }

		[Display(Name = "Chiều dài")]
		public double? Length { get; set; }

		[Display(Name = "Chiều rộng")]
		public double? Width { get; set; }

		[Display(Name = "Kết cấu")]
		public string? Structure { get; set; }
	}
}
