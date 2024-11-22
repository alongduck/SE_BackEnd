using SkyDTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyModel.Models;

[Table("AIChats")]
public class Realpty : RealptyDTO
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public new long Id { get; set; }
}