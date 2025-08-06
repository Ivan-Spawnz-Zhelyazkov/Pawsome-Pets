

namespace Pawsome_Pets.Views.CaretakingRequest
{
	public class CaretakingRequestViewModel
	{
		public int RequestId { get; set; }

		public string AnimalGiverId { get; set; }

		public string FirstName { get; set; }
		
		public string LastName { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public int AnimalId { get; set; }

		public string AnimalName { get; set; }

		public string AnimalImageUrl { get; set; }

		public string Status { get; set; }

		public DateTime SubmittedOn { get; set; }

		public string Message { get; set; } = string.Empty;

		public int Duration { get; set; }
	}
}
