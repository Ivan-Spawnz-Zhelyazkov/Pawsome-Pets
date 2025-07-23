using Pawsome_Pets.Models;
using System.Collections.Generic;
namespace Pawsome_Pets.Views.Animal
	
{
	public class AnimalListViewModel
	{
		public IEnumerable<AnimalViewModel> Animals { get; set; } = new List<AnimalViewModel>();

		public int? SelectedCategoryId { get; set; }

		public IEnumerable<Category> Categories { get; set; } = new List<Category>();

	}
}

