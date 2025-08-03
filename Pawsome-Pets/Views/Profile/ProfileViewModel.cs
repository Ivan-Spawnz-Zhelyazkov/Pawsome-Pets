using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Views.Profile
{
	public class ProfileViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		[Display(Name = "Username")]
		public string Username { get; set; } = null!;

		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		[StringLength(100, MinimumLength = 6)]
		public string? NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
		public string? ConfirmPassword { get; set; }
	}
}
