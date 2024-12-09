using SkyDTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyModel.Models
{
	public class Category : CategoryDTO
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public new long Id { get; set; }

		[Required(ErrorMessage = "{0} không được để trống")]
		[Display(Name = "Tên")]
		public new string Name { get; set; } = string.Empty;


		[InverseProperty(nameof(Product.ObjCategory))]
		public virtual ICollection<Product> Products { get; set; } = [];
	}
}
