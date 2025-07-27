using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Models
{
	public class AdoptionRequest
	{
		public int Id { get; set; }
		public int AnimalId { get; set; }
		public Animal Animal { get; set; } = null!;

		[Required]
		
		public string AdopterId { get; set; } = null!;
		public ApplicationUser Adopter { get; set; } = null!;
		public RequestStatus Status { get; set; } = RequestStatus.Pending;

		public DateTime RequestedOn { get; set; } = DateTime.UtcNow;
		public bool IsApproved { get; set; }
	}
	public enum RequestStatus
	{
		Pending,
		Approved,
		Declined
	}
}
