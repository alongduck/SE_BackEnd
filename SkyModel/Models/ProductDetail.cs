using SkyDTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyModel.Models
{
	public class ProductDetail : ProductDetailDTO
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public new long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Mô tả")]
		public new string Description { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Địa chỉ")]
		public new string Address { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Giá/m2")]
		public new double PricePerSquareMeter { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tính năng")]
		public new string Features { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Diện tích")]
		public new double Area { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Chiều dài")]
		public new double Length { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Chiều rộng")]
		public new double Width { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Kết cấu")]
		public new string Structure { get; set; } = string.Empty;

		[ForeignKey(nameof(ProductId))]
		public virtual Product ObjProduct { get; set; } = null!;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Sản phẩm")]
		public long? ProductId { get; set; }

		[InverseProperty(nameof(MinIO.ObjProductDetail))]
		public virtual ICollection<MinIO> Images { get; set; } = [];
	}
}
