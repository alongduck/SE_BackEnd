using Microsoft.EntityFrameworkCore;
using SkyDTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyModel.Models;

[Index(nameof(Name), IsUnique = false)]
public class Product : ProductDTO
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public new long Id { get; set; }

	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "Tên")]
	public new string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "Giá")]
	public new double Price { get; set; }

	[Display(Name = "Hot")]
	public new bool Hot { get; set; }

	[Display(Name = "Ngày đăng")]
	public new DateTime TimeUp { get; set; } = DateTime.Now;

	[ForeignKey(nameof(CategoryId))]
	public virtual Category ObjCategory { get; set; } = null!;

	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "Danh mục")]
	public new long? CategoryId { get; set; }

	[ForeignKey(nameof(DetailId))]
	public virtual ProductDetail ObjDetail { get; set; } = null!;

	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "Chi tiết")]
	public long? DetailId { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User ObjUser { get; set; } = null!;

	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "Người đăng")]
	public new long? UserId { get; set; }

	[ForeignKey(nameof(ThumbnailImageId))]
	public virtual MinIO? ObjThumbnailImage { get; set; }

	[Display(Name = "ID hình thumbnail")]
	public new long? ThumbnailImageId { get; set; }
}