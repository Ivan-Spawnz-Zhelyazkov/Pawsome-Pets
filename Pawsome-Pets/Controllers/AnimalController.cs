using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Animal;


namespace Pawsome_Pets.Controllers
{
	[Authorize]
	public class AnimalController : Controller
	{
		private readonly IAnimalService animalService;
		private readonly ICategoryService categoryService;

		public async Task<IActionResult> All(int? categoryId)
		{
			IEnumerable<Animal> animals;
			if (categoryId.HasValue)
			{
				animals = await animalService.GetAnimalsByCategoryAsync(categoryId.Value);
			}
			else
			{
				animals = await animalService.GetAllAnimalsAsync();
			}

			AnimalListViewModel viewModel = new AnimalListViewModel
			{
				Animals = animals,
				SelectedCategoryId = categoryId,
				Categories = await categoryService.GetAllCategoriesAsync()
			};

			return View(viewModel);
		}

		public AnimalController(IAnimalService animalService, ICategoryService categoryService)
		{
			this.animalService = animalService;
			this.categoryService = categoryService;
		}


		public async Task<IActionResult> Index(int? categoryId)
		{
			IEnumerable<Animal> animals;
			if (categoryId.HasValue)
			{
				animals = await animalService.GetAnimalsByCategoryAsync(categoryId.Value);
				ViewBag.CurrentCategoryId = categoryId.Value;
			}
			else
			{
				animals = await animalService.GetAllAnimalsAsync();
			}
			return View(animals);
		}
		// Create a new animal action
		public IActionResult Create()
		{
					return View();
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Animal animal)
		{
			if (!ModelState.IsValid)
			{
				return View(animal);
			}
			await animalService.CreateAsync(animal);
			return RedirectToAction(nameof(Index));
		}

		//Edit Existing Animal action

		public async Task<IActionResult>Edit(int id)
		{
			Animal animal = await animalService.GetAnimalByIdAsync(id);
			if (animal == null)
			{
				return NotFound();
			}
			return View(animal);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult>Edit(int id, Animal animal)
		{
			if (id != animal.Id)
			{
				return BadRequest();
			}
			if (!ModelState.IsValid)
			{
				return View(animal);
			}
			await animalService.UpdateAsync(animal);
			return RedirectToAction(nameof(Index));
		}

		//Delete Animal action

		public async Task<IActionResult> Delete(int id)
		{
			Animal animal = await animalService.GetAnimalByIdAsync(id);
			if (animal == null)
			{
				return NotFound();
			}
			return View(animal);
		}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await animalService.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}

	}
}