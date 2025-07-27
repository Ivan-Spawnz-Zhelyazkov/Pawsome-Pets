using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Animal;

namespace Pawsome_Pets.Services.Core
{
	public class AnimalService : IAnimalService
	{
		private readonly PawsomeDbContext dbContext;

		public AnimalService(PawsomeDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		// Get animals task logic
		public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
		{
			return await dbContext.Animals
				.Where(a => !a.IsDeleted)
				.Include(a => a.Category)
				.Include(a => a.Giver)
				.ToListAsync();
		}

		// Get animal by ID logic
		public async Task<Animal?> GetAnimalByIdAsync(int id)
		{
			return await dbContext.Animals
				.Where(a => !a.IsDeleted)
				.Include(a => a.Category)
				.Include(a => a.Giver)
				.FirstOrDefaultAsync(a => a.Id == id);
		}

		//Create animal logic
		public async Task CreateAsync(Animal animal)
		{
			dbContext.Animals.Add(animal);
			await dbContext.SaveChangesAsync();
		}

		// Update animal logic
		public async Task UpdateAsync(Animal animal)
		{
			dbContext.Animals.Update(animal);
			await dbContext.SaveChangesAsync();
		}

		//Delete(from site/left in DB) animal logic
		public async Task SoftDeleteAsync(int id)
		{
			Animal? animal = await dbContext.Animals.FindAsync(id);
			if (animal != null && !animal.IsDeleted)
			{
				animal.IsDeleted = true;
				await dbContext.SaveChangesAsync();

			}
		}

		// Get animals by category logic
		public async Task<IEnumerable<Animal>> GetAnimalsByCategoryAsync(int categoryId)
		{
			return await dbContext.Animals
				.Where(a => !a.IsDeleted)
				.Include(a => a.Category)
				.Where(a => a.CategoryId == categoryId)
				.ToListAsync();
		}

		// Details of an animal logic

		public async Task<AnimalDetailsViewModel?> GetDetailsAsync(int id)
		{
			return await dbContext.Animals
				.Where(a => a.Id == id)
				.Select(a => new AnimalDetailsViewModel
				{
					Id = a.Id,
					Name = a.Name,
					Breed = a.Breed,
					Age = a.Age,
					Gender = a.Gender,
					ImageUrl = a.ImageUrl,
					IsVaccinated = a.IsVaccinated,
					GiverId = a.GiverId,
					IsAdopted = a.IsAdopted,
					CategoryName = a.Category.Name,
					Description = a.Description
				})
				.FirstOrDefaultAsync();
		}
	}
}
