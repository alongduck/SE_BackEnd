using SkyDTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyModel.Models
{
	public class MinIO : MinIODTO
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public new long Id { get; set; } = DateTime.Now.Ticks;

		/// <summary>
		/// Luôn convert ToLower
		/// </summary>
		[Required(ErrorMessage = "{0} không được để trống")]
		[MaxLength(128, ErrorMessage = "Tối đa {1} ký tự")]
		[Display(Name = "Tên")]
		public new string FileName { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "ETag")]
		public new string ETag { get; set; } = string.Empty;

		/// <summary>
		/// Hệ thống tự động tính
		/// </summary>
		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Size")]
		public new long Size { get; set; }

		[Display(Name = "Ngày đăng")]
		public new DateTime TimeUp { get; set; } = DateTime.Now;

		[ForeignKey(nameof(ProductDetailId))]
		public virtual ProductDetail? ObjProductDetail { get; set; }

		public long? ProductDetailId { get; set; }
	}
}
