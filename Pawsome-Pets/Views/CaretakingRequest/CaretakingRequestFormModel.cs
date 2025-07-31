using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Views.CaretakingRequest
{
	public class CaretakingRequestFormModel
	{
		public int AnimalId { get; set; }

		[Required]		
		public string FirstName { get; set; }

		[Required]		
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[Phone]
		public string PhoneNumber { get; set; }

		[Required]
		[Display(Name = "Duration (months)")]
		public int CaretakingDuration { get; set; } // Options: 1, 3, 6, 12 months

		public string? Message { get; set; }

	}
}
