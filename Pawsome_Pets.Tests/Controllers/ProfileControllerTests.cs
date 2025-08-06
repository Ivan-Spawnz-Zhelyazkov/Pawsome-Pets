using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pawsome_Pets.Controllers;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Profile;
using System.Security.Claims;

public class ProfileControllerTests
{
	private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
	private readonly Mock<IProfileService> mockProfileService;
	private readonly Mock<IAdoptionRequestService> mockAdoptionRequestService;
	private readonly Mock<ICaretakingRequestService> mockCaretakingRequestService;

	private readonly ProfileController controller;

	public ProfileControllerTests()
	{
		mockUserManager = new Mock<UserManager<ApplicationUser>>(
			Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

		mockProfileService = new Mock<IProfileService>();
		mockAdoptionRequestService = new Mock<IAdoptionRequestService>();
		mockCaretakingRequestService = new Mock<ICaretakingRequestService>();

		controller = new ProfileController(
			mockUserManager.Object,
			mockProfileService.Object,
			mockAdoptionRequestService.Object,
			mockCaretakingRequestService.Object);
	}
	[Fact]
	public async Task Index_ShouldReturnViewWithProfileViewModel_WhenUserIsAuthenticated()
	{
		// Arrange
		var user = new ApplicationUser { UserName = "testuser" };
		var profileModel = new ProfileViewModel { Username = "testuser", Email = "test@example.com" };

		mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
					   .ReturnsAsync(user);

		mockProfileService.Setup(ps => ps.GetProfileAsync(user))
						  .ReturnsAsync(profileModel);

		// Act
		var result = await controller.Index();

		// Assert
		var viewResult = Assert.IsType<ViewResult>(result);
		var model = Assert.IsType<ProfileViewModel>(viewResult.Model);

		Assert.Equal("testuser", model.Username);
		Assert.Equal("test@example.com", model.Email);
	}
	[Fact]
	public async Task Index_ShouldReturnChallenge_WhenUserIsNull()
	{
		// Arrange
		var mockUserManager = new Mock<UserManager<ApplicationUser>>(
			Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

		mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
					   .ReturnsAsync((ApplicationUser)null); // Връщаме null за user

		var mockProfileService = new Mock<IProfileService>();
		var mockAdoptionRequestService = new Mock<IAdoptionRequestService>();
		var mockCaretakingRequestService = new Mock<ICaretakingRequestService>();

		var controller = new ProfileController(
			mockUserManager.Object,
			mockProfileService.Object,
			mockAdoptionRequestService.Object,
			mockCaretakingRequestService.Object);

		// Act
		IActionResult result = await controller.Index();

		// Assert
		Assert.IsType<ChallengeResult>(result);
	}

	[Fact]
	public async Task Update_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
	{
		// Arrange
		controller.ModelState.AddModelError("Email", "Email is required");
		var model = new ProfileViewModel();

		var user = new ApplicationUser();
		mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
					   .ReturnsAsync(user);

		// Act
		var result = await controller.Update(model);

		// Assert
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.Equal("Index", viewResult.ViewName);
		Assert.Equal(model, viewResult.Model);
	}

	[Fact]
	public async Task Update_ShouldReturnViewWithErrors_WhenUpdateFails()
	{
		// Arrange
		var user = new ApplicationUser();
		var model = new ProfileViewModel();

		mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
					   .ReturnsAsync(user);

		var failedResult = IdentityResult.Failed(new IdentityError { Description = "Update failed" });

		mockProfileService.Setup(ps => ps.UpdateProfileAsync(user, model))
						  .ReturnsAsync(failedResult);

		// Act
		var result = await controller.Update(model);

		// Assert
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.Equal("Index", viewResult.ViewName);
		Assert.Equal(model, viewResult.Model);
		Assert.False(controller.ModelState.IsValid);
		Assert.Contains(controller.ModelState[string.Empty].Errors, e => e.ErrorMessage == "Update failed");
	}

	[Fact]
	public async Task Update_ShouldRedirectToIndex_WhenUpdateSucceeds()
	{
		// Arrange
		var user = new ApplicationUser();
		var model = new ProfileViewModel();

		mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
					   .ReturnsAsync(user);

		mockProfileService.Setup(ps => ps.UpdateProfileAsync(user, model))
						  .ReturnsAsync(IdentityResult.Success);

		// Act
		var result = await controller.Update(model);

		// Assert
		var redirectResult = Assert.IsType<RedirectToActionResult>(result);
		Assert.Equal("Index", redirectResult.ActionName);
	}
}