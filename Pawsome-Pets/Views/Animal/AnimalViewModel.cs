namespace Pawsome_Pets.Views.Animal
{
	public class AnimalViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Breed { get; set; }

		public int Age { get; set; }

		public string Gender { get; set; }

		public string ImageUrl { get; set; }

		public bool IsVaccinated { get; set; }

		public string GiverId { get; set; }

		public bool IsAdopted { get; set; }

		public string CategoryName { get; set; } = null!;
	}
}
