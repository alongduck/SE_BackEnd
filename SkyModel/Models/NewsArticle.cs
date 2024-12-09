using SkyDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyModel.Models
{
	public class NewsArticle : NewsArticleDTO
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public new long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tiêu đề")]
		public new string Title { get; set; } = string.Empty;

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Nội dung")]
		public new string Content { get; set; } = string.Empty;

		[Display(Name = "Ngày đăng")]
		public new DateTime TimeUp { get; set; } = DateTime.Now;


		[InverseProperty(nameof(MinIO.ObjNewsArticle))]
		public virtual ICollection<MinIO> Images { get; set; } = [];
	}
}
