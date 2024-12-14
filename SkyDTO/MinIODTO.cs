using System;
using System.ComponentModel.DataAnnotations;

namespace SkyDTO;

public class MinIODTO
{
	public long Id { get; set; } = DateTime.Now.Ticks;

	/// <summary>
	/// luôn convert ToLower
	/// </summary>
	[Required(ErrorMessage = "{0} không được để trống")]
	[MaxLength(128, ErrorMessage = "Tối đa {1} ký tự")] // FileName
	[Display(Name = "Tên")]
	public string FileName { get; set; } = string.Empty;

	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "ETag")]
	public string ETag { get; set; } = string.Empty;

	/// <summary>
	/// Hệ thống tự động tính
	/// </summary>
	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "Size")]
	public long Size { get; set; }

	[Display(Name = "Ngày đăng")]
	public DateTime TimeUp { get; set; } = DateTime.Now;
}