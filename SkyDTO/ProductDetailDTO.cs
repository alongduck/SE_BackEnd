using System.ComponentModel.DataAnnotations;

namespace SkyDTO
{
	public class ProductDetailDTO
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Mô tả")]
		public string Description { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Địa chỉ")]
		public string Address { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Giá/m2")]
		public double PricePerSquareMeter { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tính năng")]
		public string Features { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Diện tích")]
		public double Area { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Chiều dài")]
		public double Length { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Chiều rộng")]
		public double Width { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Kết cấu")]
		public string Structure { get; set; } = string.Empty;
	}
}
