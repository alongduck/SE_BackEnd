using System.ComponentModel.DataAnnotations;

namespace SkyDTO
{
	public class UserDTO
	{
		[Required(ErrorMessage = "{0} không được để trống")]
		public long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tên")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})\b$", ErrorMessage = "{0} không đúng định dạng")]
		[Display(Name = "Số điện thoại")]
		public string PhoneNumber { get; set; } = string.Empty;

		[Display(Name = "Hình đại diện")]
		public string? Avatar { get; set; }

		[Display(Name = "Trạng thái")]
		public bool IsActive { get; set; }
	}
}
