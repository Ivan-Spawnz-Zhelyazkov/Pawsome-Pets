using Microsoft.AspNetCore.Mvc;

namespace Pawsome_Pets.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
