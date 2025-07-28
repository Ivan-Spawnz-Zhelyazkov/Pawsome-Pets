using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Models
{
	public class AdoptionRequest
	{
		public int Id { get; set; }

		public int AnimalId { get; set; }
		public Animal Animal { get; set; }

		public string AdopterId { get; set; }
		public ApplicationUser Adopter { get; set; }

		[Required]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		public string? Message { get; set; }

		public string Status { get; set; } = "Pending";

		public DateTime CreatedOn { get; set; }
	}
	public enum RequestStatus
	{
		Pending,
		Approved,
		Declined
	}

}
