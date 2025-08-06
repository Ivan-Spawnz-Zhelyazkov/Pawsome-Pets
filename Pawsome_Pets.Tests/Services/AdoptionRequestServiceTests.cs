using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Services.Core;
using Pawsome_Pets.Views.Adoption;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pawsome_Pets.Tests.Services
{
	public class AdoptionRequestServiceTests
	{
		private async Task<PawsomeDbContext> GetDatabaseContext()
		{
			var options = new DbContextOptionsBuilder<PawsomeDbContext>()
				.UseInMemoryDatabase(databaseName: $"PawsomePetsDb_{System.Guid.NewGuid()}")
				.Options;

			var dbContext = new PawsomeDbContext(options);
			dbContext.Database.EnsureCreated();

			// Seed animal
			var animal = new Animal
			{
				Id = 1,
				Name = "Max",
				Age = 3,
				Breed = "Labrador",
				Description = "Friendly dog",
				Gender = "Male",
				GiverId = "giver-123",
				ImageUrl = "https://example.com/image.jpg",
				CategoryId = 1
			};

			await dbContext.Animals.AddAsync(animal);
			await dbContext.SaveChangesAsync();

			return dbContext;
		}

		[Fact]
		public async Task CreateRequestAsync_ShouldAddRequest()
		{
			var options = new DbContextOptionsBuilder<PawsomeDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			using var context = new PawsomeDbContext(options);

			var service = new AdoptionRequestService(context);

			var animal = new Animal
			{
				Id = 1,
				Name = "Buddy",
				Age = 3,
				Description = "Friendly dog",
				Breed = "Labrador",
				Gender = "Male",
				GiverId = "giver123",
				ImageUrl = "https://example.com/dog.jpg",
				CategoryId = 1,
				IsAdopted = false
			};

			await context.Animals.AddAsync(animal);
			await context.SaveChangesAsync();

			var formModel = new AdoptionRequestFormModel
			{
				AnimalId = animal.Id,
				FullName = "John Doe",
				Email = "john@example.com",
				PhoneNumber = "123456789",
				Message = "I would love to adopt Buddy"
			};

			string testUserId = "user123";

			await service.CreateRequestAsync(formModel, testUserId);

			var savedRequest = await context.AdoptionRequests.FirstOrDefaultAsync();

			Assert.NotNull(savedRequest);
			Assert.Equal(animal.Id, savedRequest.AnimalId);
			Assert.Equal(testUserId, savedRequest.AdopterId);
			Assert.Equal(formModel.FullName, savedRequest.FullName);
			Assert.Equal(formModel.Email, savedRequest.Email);
			Assert.Equal(formModel.PhoneNumber, savedRequest.PhoneNumber);
			Assert.Equal("Pending", savedRequest.Status);
		}

		[Fact]
		public async Task GetRequestByIdAsync_ShouldReturnCorrectRequest()
		{
			var dbContext = await GetDatabaseContext();
			var service = new AdoptionRequestService(dbContext);

			var request = new AdoptionRequest
			{
				Id = 99,
				FullName = "Jane Doe",
				Email = "jane@example.com",
				PhoneNumber = "987654321",
				Message = "Looking to adopt",
				AnimalId = 1,
				AdopterId = "adopter-99"
			};

			dbContext.AdoptionRequests.Add(request);
			await dbContext.SaveChangesAsync();

			var result = await service.GetRequestByIdAsync(99);

			Assert.NotNull(result);
			Assert.Equal("Jane Doe", result.FullName);
		}

		[Fact]
		public async Task ApproveRequestAsync_ShouldSetStatusAcceptedAndMarkAnimalAdopted()
		{
			var dbContext = await GetDatabaseContext();
			var service = new AdoptionRequestService(dbContext);

			var request = new AdoptionRequest
			{
				Id = 77,
				FullName = "Alice",
				Email = "alice@example.com",
				PhoneNumber = "111222333",
				Message = "Please approve",
				AnimalId = 1,
				AdopterId = "alice123"
			};

			dbContext.AdoptionRequests.Add(request);
			await dbContext.SaveChangesAsync();

			await service.ApproveRequestAsync(77);

			var updatedRequest = dbContext.AdoptionRequests.First(r => r.Id == 77);
			var animal = dbContext.Animals.First(a => a.Id == 1);

			Assert.Equal("Accepted", updatedRequest.Status);
			Assert.True(animal.IsAdopted);
		}

		[Fact]
		public async Task DeclineRequestAsync_ShouldSetStatusDeclined()
		{
			var dbContext = await GetDatabaseContext();
			var service = new AdoptionRequestService(dbContext);

			var request = new AdoptionRequest
			{
				Id = 55,
				FullName = "Bob",
				Email = "bob@example.com",
				PhoneNumber = "999000111",
				Message = "Decline me maybe",
				AnimalId = 1,
				AdopterId = "bob123"
			};

			dbContext.AdoptionRequests.Add(request);
			await dbContext.SaveChangesAsync();

			await service.DeclineRequestAsync(55);

			var updatedRequest = dbContext.AdoptionRequests.First(r => r.Id == 55);

			Assert.Equal("Declined", updatedRequest.Status);
		}
	}
}
