using System.ComponentModel.DataAnnotations;

namespace Selu383.SP26.Api
{
	public class Location
	{
		public int Id { get; set; }
		[MaxLength(120)]
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Address { get; set; } = string.Empty;

		[Range(1, int.MaxValue)]
		public int TableCount { get; set; }

	}
}