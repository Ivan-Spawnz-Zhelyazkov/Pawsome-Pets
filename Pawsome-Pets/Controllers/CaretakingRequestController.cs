using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.CaretakingRequest;

namespace Pawsome_Pets.Controllers
{
	[Authorize(Roles = "Caretaker")]
	public class CaretakingRequestController : Controller
	{
		private readonly ICaretakingRequestService caretakingRequestService;
		private readonly UserManager<ApplicationUser> userManager;

		public CaretakingRequestController(
			ICaretakingRequestService caretakingRequestService,
			UserManager<ApplicationUser> userManager)
		{
			this.caretakingRequestService = caretakingRequestService;
			this.userManager = userManager;
		}

		[HttpGet]
		public async Task <IActionResult> Create (int animalId)
		{
			ApplicationUser user = await userManager.GetUserAsync(User);
			CaretakingRequestFormModel model = new CaretakingRequestFormModel
			{
				AnimalId = animalId,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task <IActionResult> Create(CaretakingRequestFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			ApplicationUser user = await userManager.GetUserAsync(User);
			await caretakingRequestService.CreateRequestAsync(model, user.Id);
			return RedirectToAction("MyRequests", "CaretakingRequest");
		}
		[HttpGet]
		public async Task<IActionResult> MyRequests()
		{
			string userId = userManager.GetUserId(User);

			IEnumerable<CaretakingRequestViewModel> requests = await caretakingRequestService.GetRequestByUserIdAsync(userId);

			return View("MyRequests", requests);
		}
	}
}
