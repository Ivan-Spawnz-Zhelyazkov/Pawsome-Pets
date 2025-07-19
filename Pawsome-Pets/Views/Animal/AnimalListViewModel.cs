using Pawsome_Pets.Models;
using System.Collections.Generic;
namespace Pawsome_Pets.Views.Animal
	
{
	public class AnimalListViewModel
	{
		public IEnumerable<Pawsome_Pets.Models.Animal> Animals { get; set; } = new List<Pawsome_Pets.Models.Animal>();


		public int? SelectedCategoryId { get; set; }

		public IEnumerable<Category> Categories { get; set; } = new List<Category>();
	}
}

