using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Views.Accounts
{
	public class RegisterViewModel1
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; } = null!;

	}
}
