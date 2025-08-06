using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Services.Core;
using Pawsome_Pets.Views.Adoption;
using Pawsome_Pets.Views.CaretakingRequest;
using Pawsome_Pets.Views.Profile;


namespace Pawsome_Pets.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IProfileService profileService;
		private readonly IAdoptionRequestService adoptionRequestService;
		private readonly ICaretakingRequestService caretakingRequestService;
		private readonly PawsomeDbContext dbContext;



		public ProfileController(UserManager<ApplicationUser> userManager,
			IProfileService profileService,
			IAdoptionRequestService adoptionRequestService,
			ICaretakingRequestService caretakingRequestService)
		{
			this.userManager = userManager;
			this.profileService = profileService;
			this.adoptionRequestService = adoptionRequestService;
			this.caretakingRequestService = caretakingRequestService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			ApplicationUser user = await userManager.GetUserAsync(User);
			if (user == null)
			{
				return Challenge();
			}
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
		[Authorize(Roles = "Giver")]
		public async Task<IActionResult> RequestsToMyAnimals()
		{
			string userId = userManager.GetUserId(User);

			IEnumerable<AdoptionRequestViewModel> adoptionRequests =
				await adoptionRequestService.GetRequestsToGiverAnimalsAsync(userId);

			IEnumerable<CaretakingRequestViewModel> caretakingRequests =
				await caretakingRequestService.GetRequestsToGiverAnimalsAsync(userId);

			CombinedRequestsViewModel model = new CombinedRequestsViewModel
			{
				AdoptionRequests = adoptionRequests,
				CaretakingRequests = caretakingRequests
			};

			return View("CombinedRequests", model);
		}


		//Admin panel

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AdminPanel(string adoptionStatus, string caretakingStatus, string role)
		{
			IEnumerable<AdoptionRequestViewModel> adoptionRequests = await adoptionRequestService.GetAllAsync();
			IEnumerable<CaretakingRequestViewModel> caretakingRequests = await caretakingRequestService.GetAllAsync();

			List<AdminUserViewModel> users = await userManager.Users
				.Select(u => new AdminUserViewModel
				{
					FirstName = u.FirstName,
					LastName = u.LastName,
					UserName = u.UserName,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber,
				})
				.ToListAsync();

			foreach (var user in users)
			{
				ApplicationUser identityUser = await userManager.FindByNameAsync(user.UserName);
				IList<string> roles = await userManager.GetRolesAsync(identityUser);
				user.Roles = string.Join(", ", roles);
			}

			if (!string.IsNullOrEmpty(adoptionStatus) && adoptionStatus != "All")
			{
				adoptionRequests = adoptionRequests
					.Where(r => r.Status != null && r.Status.Equals(adoptionStatus, StringComparison.OrdinalIgnoreCase));
			}


			if (!string.IsNullOrEmpty(caretakingStatus) && caretakingStatus != "All")
			{
				caretakingRequests = caretakingRequests
					.Where(r => r.Status != null && r.Status.Equals(caretakingStatus, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrEmpty(role) && role != "All")
			{
				users = users
					.Where(u => !string.IsNullOrEmpty(u.Roles) && u.Roles.Contains(role, StringComparison.OrdinalIgnoreCase))
					.ToList();
			}

			AdminPanelViewModel model = new AdminPanelViewModel
			{
				AdoptionRequests = adoptionRequests,
				CaretakingRequests = caretakingRequests,
				Users = users,
				AdoptionFilterStatus = adoptionStatus,
				CaretakingFilterStatus = caretakingStatus,
				UserRoleFilter = role
			};

			return View(model);
		}


	}
}
