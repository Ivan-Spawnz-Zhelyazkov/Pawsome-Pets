using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pawsome_Pets.Models;

namespace Pawsome_Pets.Tests
{
	public class MockUserManager : Mock<UserManager<ApplicationUser>>
	{
		public MockUserManager()
			: base(new Mock<IUserStore<ApplicationUser>>().Object,
				   null, null, null, null, null, null, null, null)
		{
		}
	}

	public class MockSignInManager : Mock<SignInManager<ApplicationUser>>
	{
		public MockSignInManager()
			: base(new Mock<UserManager<ApplicationUser>>().Object,
				   Mock.Of<IHttpContextAccessor>(),
				   Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
				   Mock.Of<IOptions<IdentityOptions>>(),
				   Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
				   Mock.Of<IAuthenticationSchemeProvider>(),
				   Mock.Of<IUserConfirmation<ApplicationUser>>())
		{
		}
	}
}
