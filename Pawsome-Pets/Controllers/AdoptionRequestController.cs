using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Adoption;

namespace Pawsome_Pets.Controllers
{
	[Authorize(Roles = "Adopter")]
	public class AdoptionRequestController : Controller
	{

		private readonly IAdoptionRequestService adoptionRequestService;
		private readonly UserManager<ApplicationUser> userManager;

		public AdoptionRequestController(
			IAdoptionRequestService adoptionRequestService,
			UserManager<ApplicationUser> userManager)
		{
			this.adoptionRequestService = adoptionRequestService;
			this.userManager = userManager;
		}
		[HttpGet]
		public async Task<IActionResult> Create (int animalId)
		{
			ApplicationUser user = await userManager.GetUserAsync(User);
			AdoptionRequestFormModel model = new AdoptionRequestFormModel
			{
				AnimalId = animalId,
				FullName = user.FirstName + " " + user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber
			};
			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create (AdoptionRequestFormModel model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}
			ApplicationUser user = await userManager.GetUserAsync(User);

			await adoptionRequestService.CreateRequestAsync(model, user.Id);

			return RedirectToAction("MyRequests","Account");
		}
		[HttpGet]
		public async Task<IActionResult> MyRequests()
		{
			ApplicationUser user = await userManager.GetUserAsync(User);
			IEnumerable<AdoptionRequestFormModel> requests = await adoptionRequestService.GetRequestsByUserIdAsync(user.Id);
			return View(requests);
		}
	}
}
