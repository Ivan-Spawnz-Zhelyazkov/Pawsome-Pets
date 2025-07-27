using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Animal;
using System.Security.Claims;


namespace Pawsome_Pets.Controllers
{
	[Authorize]
	public class AnimalController : Controller
	{
		private readonly IAnimalService animalService;
		private readonly ICategoryService categoryService;

		public async Task<IActionResult> All(int? categoryId,int page =1)
		{
			const int PageSize = 6;


			IEnumerable<Animal> animals;

			if (categoryId.HasValue)
			{
				animals = await animalService.GetAnimalsByCategoryAsync(categoryId.Value);
			}
			else
			{
				animals = (await animalService.GetAllAnimalsAsync()).ToList();
			}

			int totalAnimals = animals.Count();
			int totalPages = (int)Math.Ceiling(totalAnimals / (double)PageSize);

			List<Animal> paginatedAnimals = animals
				.Skip((page - 1) * PageSize)
				.Take(PageSize)
				.ToList();

			IEnumerable<AnimalViewModel> animalViewModels = paginatedAnimals.Select(a => new AnimalViewModel
			{
				Id = a.Id,
				Name = a.Name,
				Age = a.Age,
				Gender = a.Gender,
				Breed = a.Breed,
				IsVaccinated = a.IsVaccinated,
				ImageUrl = a.ImageUrl,
				IsAdopted = a.IsAdopted,
				CategoryName = a.Category.Name,
				GiverId = a.GiverId
			}).ToList();

			AnimalListViewModel viewModel = new AnimalListViewModel
			{
				Animals = animalViewModels,
				SelectedCategoryId = categoryId,
				Categories = await categoryService.GetAllCategoriesAsync(),
				CurrentPage = page,
				TotalPages = totalPages
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

		// Details for the pets
		public async Task<IActionResult> Details(int id)
		{
			Animal animal = await animalService.GetAnimalByIdAsync(id);
			if (animal == null)
			{
				return NotFound();
			}

			AnimalDetailsViewModel model = new AnimalDetailsViewModel
			{
				Id = animal.Id,
				Name = animal.Name,
				Breed = animal.Breed,
				Age = animal.Age,
				Gender = animal.Gender,
				ImageUrl = animal.ImageUrl,
				IsVaccinated = animal.IsVaccinated,
				GiverId = animal.GiverId,
				IsAdopted = animal.IsAdopted,
				CategoryName = animal.Category.Name,
				Description = animal.Description
			};

			string currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			ViewBag.IsAdmin = User.IsInRole("Admin");
			ViewBag.IsOwner = currentUserId != null && currentUserId == animal.GiverId;
			ViewBag.IsAdopterOrCaretaker = User.IsInRole("Adopter") || User.IsInRole("Caretaker");

			return View(model);
		}

		//Edit Existing Animal action

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			Animal? animal = await animalService.GetAnimalByIdAsync(id);
			if (animal == null)
			{
				return NotFound();
			}
			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
			bool isAdmin = User.IsInRole("Admin");
			bool isOwner = animal.GiverId == currentUserId;

			if (!isAdmin && !isOwner)
			{
				return Forbid();
			}
			var categories = await categoryService.GetAllCategoriesAsync();
			AnimalFormViewModel model = new AnimalFormViewModel
			{
				Id = animal.Id,
				Name = animal.Name,
				Age = animal.Age,
				Gender = animal.Gender,
				Breed = animal.Breed,
				Description = animal.Description,
				IsVaccinated = animal.IsVaccinated,
				CategoryId = animal.CategoryId,
				Categories = categories
								.Select(c => new SelectListItem
								{
									Value = c.Id.ToString(),
									Text = c.Name,
									Selected = c.Id == animal.CategoryId
								}).ToList()
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, AnimalFormViewModel model)
		{
			if (id != model.Id)
			{
				return BadRequest("Animal ID mismatch.");
			}
			Animal? animal = await animalService.GetAnimalByIdAsync(id);
			if (animal == null)
			{
				return NotFound();
			}

			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
			bool isAdmin = User.IsInRole("Admin");
			bool isOwner = animal.GiverId == currentUserId;
			if (!isAdmin && !isOwner)
			{
				return Forbid();
			}
			if (!ModelState.IsValid)
			{
				var categories = await categoryService.GetAllCategoriesAsync();
				model.Categories = categories
					.Select(c => new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name,
						Selected = c.Id == animal.CategoryId
					}).ToList();

				return View(model);
			}
			animal.Name = model.Name;
			animal.Age = model.Age;
			animal.Gender = model.Gender;
			animal.Breed = model.Breed;
			animal.Description = model.Description;
			animal.ImageUrl = model.ImageUrl;
			animal.IsVaccinated = model.IsVaccinated;
			animal.CategoryId = model.CategoryId;

			await animalService.UpdateAsync(animal);
			return RedirectToAction(nameof(Details), new { id = animal.Id });
		}


		//Delete Animal action

		public async Task<IActionResult> Delete(int id)
		{
			Animal? animal = await animalService.GetAnimalByIdAsync(id);
			if (animal == null || animal.IsDeleted)
			{
				return NotFound();
			}

			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
			bool isAdmin = User.IsInRole("Admin");
			bool isOwner = animal.GiverId == currentUserId;

			if (!isAdmin && !isOwner)
			{
				return Forbid();
			}
			return View(animal);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
			Animal? animal = await animalService.GetAnimalByIdAsync(id);
			if (animal == null || animal.IsDeleted)
			{
				return NotFound();
			}

			bool isAdmin = User.IsInRole("Admin");
			bool isOwner = animal.GiverId == currentUserId;

			if (!isAdmin && !isOwner)
			{
				return Forbid();
			}
			await animalService.SoftDeleteAsync(id);
			return RedirectToAction(nameof(All));
		}
	}
}