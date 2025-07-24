using Pawsome_Pets.Models;

namespace Pawsome_Pets.Services.Contracts
{
	public interface IAnimalService
	{
		Task<IEnumerable<Animal>> GetAllAnimalsAsync();
		Task<Animal?> GetAnimalByIdAsync(int id);
		Task CreateAsync(Animal animal);
		Task UpdateAsync(Animal animal);
		Task SoftDeleteAsync(int id);

		Task<IEnumerable<Animal>> GetAnimalsByCategoryAsync(int categoryId);
	}
}
