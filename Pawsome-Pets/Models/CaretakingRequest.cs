using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Models
{
	public class CaretakingRequest
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int AnimalId { get; set; }
		public Animal Animal { get; set; } = null!;
		[Required]
		public string FirstName { get; set; } = null!;
		[Required]
		public string LastName { get; set; } = null!;
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;
		[Required]
		[Phone]
		public string PhoneNumber { get; set; } = null!;
		public string? Message { get; set; } = null!;

		[Required]
		public string CaretakerId { get; set; } = null!;
		public ApplicationUser Caretaker { get; set; } = null!;

		public string Status { get; set; } = "Pending";

		[Required]
		[Range(1, 12, ErrorMessage = "Duration must be between 1 and 12 months.")]
		public int DurationMonths { get; set; }

		public DateTime StartDate { get; set; } = DateTime.UtcNow;

		public bool IsApprovedForCaretaking { get; set; } = false;
	}

}
