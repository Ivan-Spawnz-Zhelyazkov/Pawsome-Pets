using Microsoft.AspNetCore.Identity;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Profile;


namespace Pawsome_Pets.Services.Core
{
	public class ProfileService : IProfileService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public ProfileService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public async Task<ProfileViewModel> GetProfileAsync(ApplicationUser user)
		{

			return new ProfileViewModel
			{
				Email = user.Email,
				Username = user.UserName
			};
		}
		public async Task<IdentityResult> UpdateProfileAsync(ApplicationUser user, ProfileViewModel model)
		{
			user.Email = model.Email;
			user.UserName = model.Username;

			IdentityResult result = await userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				await signInManager.RefreshSignInAsync(user);
			}

			return result;
		}
	}
}