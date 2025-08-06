using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services;
using Pawsome_Pets.Services.Core;
using Pawsome_Pets.Views.CaretakingRequest;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pawsome_Pets.Tests.Services
{
	public class CaretakingRequestServiceTests
	{
		private PawsomeDbContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<PawsomeDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var dbContext = new PawsomeDbContext(options);
			return dbContext;
		}

		[Fact]
		public async Task CreateRequestAsync_ShouldAddNewCaretakingRequest()
		{

			PawsomeDbContext dbContext = GetDbContext();
			CaretakingRequestService service = new CaretakingRequestService(dbContext);

			Animal animal = new Animal
			{
				Id = 1,
				Name = "Leo",
				Age = 4,
				Description = "Calm cat",
				Breed = "British Shorthair",
				Gender = "Female",
				GiverId = "giver123",
				ImageUrl = "https://example.com/cat.jpg",
				CategoryId = 2,
				IsAdopted = false
			};

			await dbContext.Animals.AddAsync(animal);
			await dbContext.SaveChangesAsync();

			CaretakingRequestFormModel model = new CaretakingRequestFormModel
			{
				AnimalId = 1,
				FirstName = "Alice",
				LastName = "Smith",
				Email = "alice@example.com",
				PhoneNumber = "0888123456",
				CaretakingDuration = 6,
				Message = "I'd love to take care of Leo"
			};

			string userId = "user123";

			await service.CreateRequestAsync(model, userId);

			CaretakingRequest? request = await dbContext.CaretakingRequests.FirstOrDefaultAsync();

			Assert.NotNull(request);
			Assert.Equal(model.AnimalId, request.AnimalId);
			Assert.Equal(model.FirstName, request.FirstName);
			Assert.Equal(model.LastName, request.LastName);
			Assert.Equal(model.Email, request.Email);
			Assert.Equal(model.PhoneNumber, request.PhoneNumber);
			Assert.Equal(model.CaretakingDuration, request.DurationMonths);
			Assert.Equal("Pending", request.Status);
		}

		[Fact]
		public async Task ApproveRequestAsync_ShouldSetStatusToApproved()
		{
			var dbContext = GetDbContext();
			var service = new CaretakingRequestService(dbContext);

			var animal = new Animal
			{
				Id = 2,
				Name = "Milo",
				Age = 5,
				Description = "Energetic dog",
				Breed = "Beagle",
				Gender = "Male",
				GiverId = "giver456",
				ImageUrl = "https://example.com/milo.jpg",
				CategoryId = 1
			};

			CaretakingRequest request = new CaretakingRequest
			{
				Animal = animal,
				AnimalId = animal.Id,
				FirstName = "John",
				LastName = "Doe",
				Email = "john@example.com",
				PhoneNumber = "1234567890",
				DurationMonths = 3,
				Message = "I'd love to help.",
				StartDate = DateTime.UtcNow,
				CaretakerId = "caretaker-user-id",
				Status = "Pending"
			};

			await dbContext.Animals.AddAsync(animal);
			await dbContext.CaretakingRequests.AddAsync(request);
			await dbContext.SaveChangesAsync();

			await service.ApproveRequestAsync(request.Id);

			var updatedRequest = await dbContext.CaretakingRequests.FindAsync(request.Id);
			Assert.Equal("Approved", updatedRequest.Status);
		}

		[Fact]
		public async Task DeclineRequestAsync_ShouldSetStatusToDeclined()
		{
			var dbContext = GetDbContext();
			var service = new CaretakingRequestService(dbContext);

			CaretakingRequest request = new CaretakingRequest
			{
				Id = 2,
				AnimalId = 1,
				Animal = new Animal
				{
					Id = 1,
					Name = "Mimi",
					Breed = "Cat",
					Age = 2,
					CategoryId = 1,
					Description = "Cute cat",
					Gender = "Female",
					GiverId = "giver2",
					ImageUrl = "https://example.com/mimi.jpg"
				},
				FirstName = "Maria",
				LastName = "Petrova",
				Email = "maria@petrova.com",
				PhoneNumber = "0888111222",
				Message = "I'd like to help",
				DurationMonths = 1,
				IsApprovedForCaretaking = false,
				CaretakerId = "caretaker456"
			};

			dbContext.CaretakingRequests.Add(request);
			await dbContext.SaveChangesAsync();

			// Act
			await service.DeclineRequestAsync(request.Id);

			// Assert
			var declined = await dbContext.CaretakingRequests.FindAsync(request.Id);
			Assert.Null(declined);
		}


		[Fact]
		public async Task GetRequestByIdAsync_ShouldReturnCorrectRequest()
		{
			var dbContext = GetDbContext();
			var service = new CaretakingRequestService(dbContext);
			CaretakingRequest request = new CaretakingRequest
			{
				Id = 3,
				AnimalId = 2,
				Animal = new Animal
				{
					Id = 2,
					Name = "Rex",
					Breed = "German Shepherd",
					Age = 5,
					CategoryId = 2,
					Description = "Guard dog",
					Gender = "Male",
					GiverId = "giver3",
					ImageUrl = "https://example.com/rex.jpg"
				},
				FirstName = "Nikolay",
				LastName = "Nikolov",
				Email = "nik@nikolov.com",
				PhoneNumber = "0888000999",
				Message = "Can help evenings",
				DurationMonths = 6,
				IsApprovedForCaretaking = true,
				CaretakerId = "caretaker789"
			};

			dbContext.CaretakingRequests.Add(request);
			await dbContext.SaveChangesAsync();

			var result = await service.GetRequestByIdAsync(request.Id);

			Assert.NotNull(result);
			Assert.Equal("Rex", result.AnimalName);
			Assert.Equal("Nikolay", result.FirstName);
		}

	}
}
