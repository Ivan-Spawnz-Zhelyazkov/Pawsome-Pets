namespace Pawsome_Pets.Models
{
	public class Animal
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;
		public int Age { get; set; }
		public string Description { get; set; } = null!;
		public string ImageUrl { get; set; } = null!;
		public bool IsAdopted { get; set; } = false;

		//Categories
		public int CategoryId { get; set; }
		public Category Category { get; set; } = null!;

		
		public string GiverId { get; set; } = null!;
		public ApplicationUser Giver { get; set; } = null!;

		public string? AdopterId { get; set; }
		public ApplicationUser Adopter { get; set; } = null!;

		// Adoption requests
		public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
		// Caretaking requests
		public ICollection<CaretakingRequest> CaretakingRequests { get; set; } = new List<CaretakingRequest>();
	}
}
