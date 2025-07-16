using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Models
{
	public class AdoptionRequest
	{
		public int Id { get; set; }

		[Required]
		public int AnimalId { get; set; }
		public Animal Animal { get; set; } = null!;

		[Required]
		
		public string AdopterId { get; set; } = null!;
		public ApplicationUser Adopter { get; set; } = null!;

		public DateTime RequestedOn { get; set; } = DateTime.UtcNow;
		public bool IsApproved { get; set; }
	}
}
