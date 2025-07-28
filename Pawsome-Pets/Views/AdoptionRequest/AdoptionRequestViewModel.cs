namespace Pawsome_Pets.Views.Adoption
{
	public class AdoptionRequestViewModel
	{
		public int Id { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string? Message { get; set; }

		public string AnimalName { get; set; }

		public string AnimalImageUrl { get; set; }

		public string Status { get; set; }

		public DateTime SubmittedOn{ get; set; }
	}
}
