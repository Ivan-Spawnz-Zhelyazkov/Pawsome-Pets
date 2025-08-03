using Pawsome_Pets.Views.Profile;
using Pawsome_Pets.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Pawsome_Pets.Services.Contracts
{
	public interface IProfileService
	{

		Task<ProfileViewModel> GetProfileAsync(ApplicationUser user);
		Task <IdentityResult> UpdateProfileAsync(ApplicationUser user, ProfileViewModel model);
	}
}
