using System;

namespace SkyDTO.Commons
{
	public class ImageDTO
	{
		public long Id { get; set; } = DateTime.Now.Ticks;

		public string? Src { get; set; }

		public string? Alt { get; set; }
	}
}
