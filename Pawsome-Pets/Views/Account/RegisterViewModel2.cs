using System.ComponentModel.DataAnnotations;

namespace Pawsome_Pets.Views.Accounts
{
	public class RegisterViewModel2	
	{
		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = null!;

		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = null!;

		[Required]
		public string UserName { get; set; } = null!;

		[Required]
		[Display(Name = "Role")]
		public string Role { get; set; } = null!;

		[Phone]
		[Display(Name = "Phone Number")]
		public string? PhoneNumber { get; set; }
	}
}
