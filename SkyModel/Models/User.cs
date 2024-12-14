using SkyDTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyModel.Models;

public class User : UserDTO
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Required(ErrorMessage = "{0} không được để trống")]
	public new long Id { get; set; }

	[Required(ErrorMessage = "{0} không được để trống")]
	[Display(Name = "Tên")]
	public new string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "{0} không được để trống")]
	[RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})\b$", ErrorMessage = "{0} không đúng định dạng")]
	[Display(Name = "Số điện thoại")]
	public new string PhoneNumber { get; set; } = string.Empty;

	[Display(Name = "Hình đại diện")]
	public new string? Avatar { get; set; }

	[Display(Name = "Trạng thái")]
	public new bool IsActive { get; set; } = true;

	[Display(Name = "Role")]
	public UserRole Role { get; set; } = UserRole.User;

	[Display(Name = "Ngày tạo")]
	public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum UserRole
{
	User = 1,
	Admin = 2
}