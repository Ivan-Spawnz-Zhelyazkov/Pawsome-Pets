using Pawsome_Pets.Models;
using System.Collections.Generic;
namespace Pawsome_Pets.Views.Animal
	// For Sorting animals and page pagination
{
	public class AnimalListViewModel
	{
		public IEnumerable<AnimalViewModel> Animals { get; set; } = new List<AnimalViewModel>();

		public int? SelectedCategoryId { get; set; }

		public IEnumerable<Category> Categories { get; set; } = new List<Category>();



		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
	}
}

