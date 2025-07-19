using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Models;

namespace Pawsome_Pets.Services.Core
{
	public class CategoryService : ICategoryService
	{
		private readonly PawsomeDbContext dbContext;

		public CategoryService(PawsomeDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			return await dbContext.Categories.ToListAsync();
		}
	}
}
