using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;

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
				.Include(a => a.Category)
				.Include(a => a.Giver)
				.ToListAsync();
		}

		// Get animal by ID logic
		public async Task<Animal?> GetAnimalByIdAsync(int id)
		{
			return await dbContext.Animals
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

		//Delete animal logic
		public async Task DeleteAsync(int id)
		{
			Animal? animal = await dbContext.Animals.FindAsync(id);
			if (animal != null)
			{
				dbContext.Animals.Remove(animal);
				await dbContext.SaveChangesAsync();
			}
		}

		// Get animals by category logic
		public async Task<IEnumerable<Animal>> GetAnimalsByCategoryAsync(int categoryId)
		{
			return await dbContext.Animals
				.Include(a => a.Category)
				.Where(a => a.CategoryId == categoryId)
				.ToListAsync();
		}
	}
}
