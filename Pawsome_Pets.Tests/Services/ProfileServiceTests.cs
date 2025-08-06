using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Core;
using Pawsome_Pets.Views.Profile;
using System.Threading.Tasks;
using Xunit;

namespace Pawsome_Pets.Tests.Services
{
	public class ProfileServiceTests
	{
		private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
		private readonly Mock<SignInManager<ApplicationUser>> signInManagerMock;
		private readonly ProfileService profileService;

		public ProfileServiceTests()
		{
			var store = new Mock<IUserStore<ApplicationUser>>();
			userManagerMock = new Mock<UserManager<ApplicationUser>>(
				store.Object, null, null, null, null, null, null, null, null);

			var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
			var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

			signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
				userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

			profileService = new ProfileService(userManagerMock.Object, signInManagerMock.Object);
		}

		[Fact]
		public async Task GetProfileAsync_ShouldReturnCorrectViewModel()
		{

			ApplicationUser user = new ApplicationUser
			{
				Email = "test@example.com",
				UserName = "TestUser"
			};


			ProfileViewModel result = await profileService.GetProfileAsync(user);


			Assert.Equal("test@example.com", result.Email);
			Assert.Equal("TestUser", result.Username);
		}

		[Fact]
		public async Task UpdateProfileAsync_ShouldUpdateUserAndRefreshSignIn()
		{

			ApplicationUser user = new ApplicationUser
			{
				Email = "old@example.com",
				UserName = "OldUser"
			};

			ProfileViewModel model = new ProfileViewModel
			{
				Email = "new@example.com",
				Username = "NewUser"
			};

			userManagerMock.Setup(m => m.UpdateAsync(user))
				.ReturnsAsync(IdentityResult.Success);

			signInManagerMock.Setup(m => m.RefreshSignInAsync(user))
				.Returns(Task.CompletedTask);


			var result = await profileService.UpdateProfileAsync(user, model);


			Assert.True(result.Succeeded);
			Assert.Equal("new@example.com", user.Email);
			Assert.Equal("NewUser", user.UserName);

			userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);
			signInManagerMock.Verify(m => m.RefreshSignInAsync(user), Times.Once);
		}
		[Fact]
		public async Task GetProfileAsync_ShouldReturnCorrectProfile()
		{
			ApplicationUser user = new ApplicationUser
			{
				UserName = "testuser",
				Email = "testuser@example.com"
			};
			ProfileService service = new ProfileService(null, null);


			ProfileViewModel result = await service.GetProfileAsync(user);

			Assert.Equal("testuser", result.Username);
			Assert.Equal("testuser@example.com", result.Email);
		}


		[Fact]
		public async Task UpdateProfileAsync_ShouldReturnSuccess_WhenUpdateSucceeds()
		{
			var mockUserManager = new Mock<UserManager<ApplicationUser>>(
				Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

			var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
				mockUserManager.Object,
				Mock.Of<IHttpContextAccessor>(),
				Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
				null, null, null, null);

			var user = new ApplicationUser
			{
				UserName = "olduser",
				Email = "old@example.com"
			};

			mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
						   .ReturnsAsync(IdentityResult.Success);

			mockSignInManager.Setup(sm => sm.RefreshSignInAsync(user))
							 .Returns(Task.CompletedTask);

			var service = new ProfileService(mockUserManager.Object, mockSignInManager.Object);

			var model = new ProfileViewModel
			{
				Username = "newuser",
				Email = "new@example.com"
			};

			var result = await service.UpdateProfileAsync(user, model);


			Assert.True(result.Succeeded);
			Assert.Equal("newuser", user.UserName);
			Assert.Equal("new@example.com", user.Email);

			mockUserManager.Verify(um => um.UpdateAsync(user), Times.Once);
			mockSignInManager.Verify(sm => sm.RefreshSignInAsync(user), Times.Once);
		}

	}
}