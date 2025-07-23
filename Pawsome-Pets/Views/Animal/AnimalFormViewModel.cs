namespace Pawsome_Pets.Views.Animal
{
	using System.ComponentModel.DataAnnotations;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public class AnimalFormViewModel
	{
		[Required]
		public string Name { get; set; } = null!;

		[Range(0, 100)]
		public int Age { get; set; }

		[Required]
		public string Gender { get; set; } = null!;

		[Required]
		public string Breed { get; set; } = null!;

		[Display(Name = "Is Vaccinated")]
		public bool IsVaccinated { get; set; }

		[Required]
		public string Description { get; set; } = null!;

		[Display(Name = "Image URL")]
		[Url]
		[Required]
		public string ImageUrl { get; set; } = null!;

		[Display(Name = "Category")]
		[Required]
		public int CategoryId { get; set; }

		public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
	}

}
