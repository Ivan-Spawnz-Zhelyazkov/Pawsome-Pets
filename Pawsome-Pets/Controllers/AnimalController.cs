using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

			IEnumerable<AnimalViewModel> animalViewModels = animals.Select(a => new AnimalViewModel
			{
				Id = a.Id,
				Name = a.Name,
				Age = a.Age,
				Gender = a.Gender,
				Breed = a.Breed,
				IsVaccinated = a.IsVaccinated,
				ImageUrl = a.ImageUrl,
				IsAdopted = a.IsAdopted,
				CategoryName = a.Category.Name
			}).ToList();

			AnimalListViewModel viewModel = new AnimalListViewModel
			{
				Animals = animalViewModels,
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

		//Add an Animal

		[Authorize(Roles = "Admin,Giver")]
		public async Task<IActionResult> Add()
		{
			AnimalFormViewModel model = new AnimalFormViewModel
			{
				Categories = (await categoryService.GetAllCategoriesAsync())
					.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
			};

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Giver")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(AnimalFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = (await categoryService.GetAllCategoriesAsync())
					.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
				return View(model);
			}

			Animal animal = new Animal
			{
				Name = model.Name,
				Age = model.Age,
				Gender = model.Gender,
				Breed = model.Breed,
				IsVaccinated = model.IsVaccinated,
				Description = model.Description,
				ImageUrl = model.ImageUrl,
				CategoryId = model.CategoryId,
				GiverId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
			};

			await animalService.CreateAsync(animal);

			return RedirectToAction(nameof(All));
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