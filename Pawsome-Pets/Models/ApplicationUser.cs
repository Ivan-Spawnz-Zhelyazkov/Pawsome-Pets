using Microsoft.AspNetCore.Identity;

namespace Pawsome_Pets.Models
{
	public class ApplicationUser :IdentityUser
	{
		public string? FirstName { get; set; } = null!;
		public string? LastName { get; set; } = null!;

		// animals given for adoption
		public ICollection<Animal> GivenAnimals { get; set; } = new List<Animal>();

		// animals adopted by user
		public ICollection<Animal> AdoptedAnimals { get; set; } = new List<Animal>();

		//AdoptionRequests
		public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();

		// CaretakingRequests
		public ICollection<CaretakingRequest> CaretakingRequests { get; set; } = new List<CaretakingRequest>();
	}
}
