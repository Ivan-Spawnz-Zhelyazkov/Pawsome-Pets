using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Services.Core;
using Pawsome_Pets.Views.Profile;


namespace Pawsome_Pets.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IProfileService profileService;



		public ProfileController(UserManager<ApplicationUser> userManager, IProfileService profileService)
		{
			this.userManager = userManager;
			this.profileService = profileService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			ApplicationUser user = await userManager.GetUserAsync(User);
			ProfileViewModel model = await profileService.GetProfileAsync(user);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(ProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("Index", model);
			}

			ApplicationUser user = await userManager.GetUserAsync(User);
			IdentityResult result = await profileService.UpdateProfileAsync(user, model);

			if (result.Succeeded)
			{
				return RedirectToAction(nameof(Index));
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View("Index", model);
		}
		[HttpGet]
		public async Task<IActionResult> MyRequests()
		{
			ApplicationUser user = await userManager.GetUserAsync(User);
			IList<string> roles = await userManager.GetRolesAsync(user);

			if (roles.Contains("Admin"))
			{
				return RedirectToAction("All", "AdoptionRequest");
			}
			else if (roles.Contains("Giver"))
			{
				return RedirectToAction("RequestsToMyAnimals", "AdoptionRequest");
			}
			else if (roles.Contains("Adopter"))
			{
				return RedirectToAction("MyRequests", "AdoptionRequest");
			}
			else if (roles.Contains("Caretaker"))
			{
				return RedirectToAction("MyRequests", "CaretakingRequest");
			}

			return RedirectToAction("Index");
		}

	}
}
