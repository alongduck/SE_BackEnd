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

		[Display(Name = "Mô tả")]
		public new string? Description { get; set; }

		[Display(Name = "Địa chỉ")]
		public new string? Address { get; set; }

		[Display(Name = "Giá/m2")]
		public new double? PricePerSquareMeter { get; set; }

		[Display(Name = "Tính năng")]
		public new string? Features { get; set; }

		[Display(Name = "Diện tích")]
		public new double? Area { get; set; }

		[Display(Name = "Chiều dài")]
		public new double? Length { get; set; }

		[Display(Name = "Chiều rộng")]
		public new double? Width { get; set; }

		[Display(Name = "Kết cấu")]
		public new string? Structure { get; set; }

		[InverseProperty(nameof(MinIO.ObjProductDetail))]
		public virtual ICollection<MinIO> Images { get; set; } = [];
	}
}
