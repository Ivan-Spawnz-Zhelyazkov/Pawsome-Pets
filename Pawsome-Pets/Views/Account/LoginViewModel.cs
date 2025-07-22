using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Views.Account
{
	public class LoginViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		public bool RememberMe { get; set; }
	}
}
