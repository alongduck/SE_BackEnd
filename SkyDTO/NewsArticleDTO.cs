using System;
using System.ComponentModel.DataAnnotations;

namespace SkyDTO
{
	public class NewsArticleDTO
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tiêu đề")]
		public string Title { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Nội dung")]
		public string Content { get; set; } = string.Empty;

		[Display(Name = "Ngày đăng")]
		public DateTime TimeUp { get; set; } = DateTime.Now;
	}
}
