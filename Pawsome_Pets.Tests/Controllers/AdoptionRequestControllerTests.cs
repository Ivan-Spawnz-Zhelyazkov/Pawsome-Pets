using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pawsome_Pets.Controllers;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Adoption;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Pawsome_Pets.Tests.Controllers
{
	public class AdoptionRequestControllerTests
	{
		private Mock<IAdoptionRequestService> mockService;
		private Mock<UserManager<ApplicationUser>> mockUserManager;

		public AdoptionRequestControllerTests()
		{
			mockService = new Mock<IAdoptionRequestService>();

			mockUserManager = new Mock<UserManager<ApplicationUser>>(
				Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
		}
		[Fact]
		public async Task Approve_ShouldCallServiceAndRedirect()
		{
			int requestId = 1;
			mockService.Setup(s => s.ApproveRequestAsync(requestId)).Returns(Task.CompletedTask);

			var controller = new AdoptionRequestController(mockService.Object, mockUserManager.Object);

			var result = await controller.Approve(requestId);

			mockService.Verify(s => s.ApproveRequestAsync(requestId), Times.Once);
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("RequestsToMyAnimals", redirectResult.ActionName);
			Assert.Equal("RequestsToMyAnimals", redirectResult.ActionName);
			Assert.Equal("AdoptionRequest", redirectResult.ControllerName);
		}
		[Fact]
		public async Task Decline_ShouldCallServiceAndRedirect()
		{

			int requestId = 1;
			mockService.Setup(s => s.DeclineRequestAsync(requestId)).Returns(Task.CompletedTask);

			var controller = new AdoptionRequestController(mockService.Object, mockUserManager.Object);


			var result = await controller.Decline(requestId);


			mockService.Verify(s => s.DeclineRequestAsync(requestId), Times.Once);
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("RequestsToMyAnimals", redirectResult.ActionName);
			Assert.Equal("RequestsToMyAnimals", redirectResult.ActionName);
			Assert.Equal("AdoptionRequest", redirectResult.ControllerName);
		}
		[Fact]
		public async Task Details_ShouldReturnViewWithModel()
		{
			int requestId = 1;

			var mockService = new Mock<IAdoptionRequestService>();
			var mockUserManager = new Mock<UserManager<ApplicationUser>>(
				Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

			var testUser = new ApplicationUser { Id = "giver-user-id" };

			mockUserManager
				.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
				.ReturnsAsync(testUser);

			var expectedModel = new AdoptionRequestViewModel
			{
				Id = requestId,
				FullName = "Test User",
				Status = "Pending",
				GiverId = "giver-user-id"
			};

			mockService.Setup(s => s.GetRequestByIdAsync(requestId))
					   .ReturnsAsync(expectedModel);

			var controller = new AdoptionRequestController(mockService.Object, mockUserManager.Object);

			IActionResult result = await controller.Details(requestId);

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<AdoptionRequestViewModel>(viewResult.Model);

			Assert.Equal(expectedModel.Id, model.Id);
			Assert.Equal(expectedModel.FullName, model.FullName);
			Assert.Equal(expectedModel.Status, model.Status);
		}
	}
}
