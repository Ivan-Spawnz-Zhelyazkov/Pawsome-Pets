using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pawsome_Pets.Models;
using Pawsome_Pets.Views.Accounts;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreGeneratedDocument;


namespace Pawsome_Pets.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}


		//Step One of Registration
		[HttpGet]
		public IActionResult RegisterStepOne()
		{
			return View();
		}

		[HttpPost]
		public IActionResult RegisterStepOne(RegisterViewModel1 model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			HttpContext.Session.SetString("Email", model.Email);
			HttpContext.Session.SetString("Password", model.Password);

			return RedirectToAction(nameof(RegisterStepTwo));
		}

		//Step Two of Registration
		[HttpGet]
		public IActionResult RegisterStepTwo()
		{
			ViewBag.Roles = new[] { "Giver", "Adopter", "Caretaker" };
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RegisterStepTwo(RegisterViewModel2 model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Roles = new[] { "Giver", "Adopter", "Caretaker" };
				return View(model);
			}

			string? email = HttpContext.Session.GetString("Email");
			string? password = HttpContext.Session.GetString("Password");

			if (email == null || password == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid registration data.");
				return RedirectToAction(nameof(RegisterStepOne));
			}
			ApplicationUser user = new ApplicationUser
			{
				UserName = model.UserName,
				FirstName = model.FirstName,
				LastName = model.LastName,
				PhoneNumber = model.PhoneNumber,
			};
			IdentityResult result = await userManager.CreateAsync(user, password);

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, model.Role);
				await signInManager.SignInAsync(user, isPersistent: false);
				return RedirectToAction("Index", "Home");
			}

			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}



	}
}
