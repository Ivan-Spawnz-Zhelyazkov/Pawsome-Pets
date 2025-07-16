using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Models
{
	public class CaretakingRequest
	{
		public int Id { get; set; }

		[Required]
		public int AnimalId { get; set; }
		public Animal Animal { get; set; } = null!;

		[Required]
		public string CaretakerId { get; set; } = null!;
		public ApplicationUser Caretaker { get; set; } = null!;

		[Required]
		public int DurationMonths { get; set; }
		public DateTime StartDate { get; set; } = DateTime.UtcNow;
		public bool IsApproved { get; set; }
	}
}
