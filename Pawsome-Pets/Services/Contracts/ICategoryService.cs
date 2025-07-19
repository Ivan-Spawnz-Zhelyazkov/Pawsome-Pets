using Pawsome_Pets.Models;

namespace Pawsome_Pets.Services.Contracts
{
	public interface ICategoryService
	{
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
	}
}
