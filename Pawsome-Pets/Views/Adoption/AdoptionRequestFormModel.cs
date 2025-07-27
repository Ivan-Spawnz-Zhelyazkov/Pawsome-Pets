using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Views.Adoption
{
	public class AdoptionRequestFormModel
	{
		public int AnimalId { get; set; }

		[Required]
		public string FullName { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		[Phone]
		public string PhoneNumber { get; set; }

		[Display(Name = "Why do you want to adopt? (Optional)")]
		public string? Message { get; set; }
	}
}
