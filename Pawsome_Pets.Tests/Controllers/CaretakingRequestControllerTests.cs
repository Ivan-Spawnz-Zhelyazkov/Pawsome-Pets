using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pawsome_Pets.Controllers;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.CaretakingRequest;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

public class CaretakingRequestControllerTests
{
	private readonly Mock<ICaretakingRequestService> mockService;
	private readonly Mock<UserManager<ApplicationUser>> mockUserManager;

	public CaretakingRequestControllerTests()
	{
		mockService = new Mock<ICaretakingRequestService>();

		mockUserManager = new Mock<UserManager<ApplicationUser>>(
			Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
	}

	[Fact]
	public async Task Details_ShouldReturnViewWithModel_WhenRequestExistsAndUserIsAuthorized()
	{
		int requestId = 1;

		var testUser = new ApplicationUser { Id = "giver-user-id" };
		var expectedModel = new CaretakingRequestViewModel
		{
			RequestId = requestId,
			FirstName = "Caretaker",
			LastName = "User",
			Status = "Pending",

		};

		mockUserManager
			.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
			.ReturnsAsync(testUser);

		mockService.Setup(s => s.GetRequestByIdAsync(requestId))
				   .ReturnsAsync(expectedModel);

		var controller = new CaretakingRequestController(mockService.Object, mockUserManager.Object);

		IActionResult result = await controller.Details(requestId);

		var viewResult = Assert.IsType<ViewResult>(result);
		var model = Assert.IsType<CaretakingRequestViewModel>(viewResult.Model);

		Assert.Equal(expectedModel.RequestId, model.RequestId);
		Assert.Equal(expectedModel.FirstName, model.FirstName);
		Assert.Equal(expectedModel.Status, model.Status);
	}

	[Fact]
	public async Task Details_ShouldReturnNotFound_WhenRequestDoesNotExist()
	{
		int requestId = 1;

		mockService.Setup(s => s.GetRequestByIdAsync(requestId))
				   .ReturnsAsync((CaretakingRequestViewModel)null);

		var controller = new CaretakingRequestController(mockService.Object, mockUserManager.Object);

		IActionResult result = await controller.Details(requestId);

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Details_ShouldReturnForbid_WhenUserIsNotAuthorized()
	{
		int requestId = 1;

		var mockService = new Mock<ICaretakingRequestService>();
		var mockUserManager = new Mock<UserManager<ApplicationUser>>(
			Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

		var model = new CaretakingRequestViewModel
		{
			RequestId = requestId,
			AnimalGiverId = "giver-123"
		};

		mockService.Setup(s => s.GetRequestByIdAsync(requestId))
				   .ReturnsAsync(model);

		var unauthorizedUser = new ApplicationUser { Id = "wrong-user" };
		mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
					   .ReturnsAsync(unauthorizedUser);

		var controller = new CaretakingRequestController(mockService.Object, mockUserManager.Object);

		// Тук добавяме User в controller:
		var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
		{
		new Claim(ClaimTypes.NameIdentifier, "wrong-user")
		}, "mock"));

		controller.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		IActionResult result = await controller.Details(requestId);

		Assert.IsType<ForbidResult>(result);
	}

	[Fact]
	public async Task Approve_ShouldCallServiceAndRedirect()
	{
		int requestId = 1;

		mockService.Setup(s => s.ApproveRequestAsync(requestId))
				   .Returns(Task.CompletedTask)
				   .Verifiable();

		var controller = new CaretakingRequestController(mockService.Object, mockUserManager.Object);

		IActionResult result = await controller.Approve(requestId);

		mockService.Verify(s => s.ApproveRequestAsync(requestId), Times.Once);

		var redirectResult = Assert.IsType<RedirectToActionResult>(result);
		Assert.Equal("RequestsToMyAnimals", redirectResult.ActionName);
		Assert.Equal("CaretakingRequest", redirectResult.ControllerName);
	}

	[Fact]
	public async Task Decline_ShouldCallServiceAndRedirect()
	{
		int requestId = 1;

		mockService.Setup(s => s.DeclineRequestAsync(requestId))
				   .Returns(Task.CompletedTask)
				   .Verifiable();

		var controller = new CaretakingRequestController(mockService.Object, mockUserManager.Object);

		IActionResult result = await controller.Decline(requestId);

		mockService.Verify(s => s.DeclineRequestAsync(requestId), Times.Once);

		var redirectResult = Assert.IsType<RedirectToActionResult>(result);
		Assert.Equal("RequestsToMyAnimals", redirectResult.ActionName);
		Assert.Equal("CaretakingRequest", redirectResult.ControllerName);
	}	
}