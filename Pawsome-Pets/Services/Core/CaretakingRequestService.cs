
using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.CaretakingRequest;

namespace Pawsome_Pets.Services.Core
{
	public class CaretakingRequestService : ICaretakingRequestService
	{
		private readonly PawsomeDbContext dbContext;

		public CaretakingRequestService(PawsomeDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task CreateRequestAsync(CaretakingRequestFormModel model, string userId)
		{
			CaretakingRequest request = new CaretakingRequest
			{
				AnimalId = model.AnimalId,
				CaretakerId = userId,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				Message = model.Message,
				DurationMonths = model.CaretakingDuration,
				StartDate = DateTime.UtcNow,
				IsApprovedForCaretaking = false
			};

			dbContext.CaretakingRequests.Add(request);
			await dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<CaretakingRequestViewModel>> GetRequestByUserIdAsync(string userId)
		{
			return await dbContext.CaretakingRequests
				.Where(r => r.CaretakerId == userId)
				.Include(r => r.Animal)
				.Select(r => new CaretakingRequestViewModel
				{
					RequestId = r.Id,
					AnimalId = r.AnimalId,
					AnimalName = r.Animal.Name,
					AnimalImageUrl = r.Animal.ImageUrl,
					Status = r.IsApprovedForCaretaking ? "Approved" : "Pending",
					SubmittedOn = r.StartDate,
					Duration = r.DurationMonths
				})
				.ToListAsync();
		}

		public async Task ApproveRequestAsync(int requestId)
		{
			CaretakingRequest? request = await dbContext.CaretakingRequests
				.Include(r => r.Animal)
				.FirstOrDefaultAsync(r => r.Id == requestId);

			if (request != null)
			{
				request.IsApprovedForCaretaking = true;
				request.Status = "Approved";
				await dbContext.SaveChangesAsync();
			}
		}

		public async Task DeclineRequestAsync(int requestId)
		{
			CaretakingRequest? request = await dbContext.CaretakingRequests
				.FirstOrDefaultAsync(r => r.Id == requestId);

			if (request != null)
			{
				request.IsApprovedForCaretaking = false;
				request.Status = "Declined";
				dbContext.CaretakingRequests.Remove(request);
				await dbContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<CaretakingRequestViewModel>> GetRequestsToGiverAnimalsAsync(string giverId)
		{
			return await dbContext.CaretakingRequests
				.Where(r => r.Animal.GiverId == giverId)
				.Include(r => r.Animal)
				.Select(r => new CaretakingRequestViewModel
				{
					RequestId = r.Id,
					AnimalId = r.AnimalId,
					AnimalName = r.Animal.Name,
					AnimalImageUrl = r.Animal.ImageUrl,
					Status = r.IsApprovedForCaretaking ? "Approved" : "Pending",
					SubmittedOn = r.StartDate,
					Message = r.Message,
					Duration = r.DurationMonths,
					FirstName = r.FirstName,
					LastName = r.LastName,
					Email = r.Email,
					PhoneNumber = r.PhoneNumber
				})
				.ToListAsync();
		}
		public async Task<CaretakingRequestViewModel?> GetRequestByIdAsync(int id)
		{
			CaretakingRequest? request = await dbContext.CaretakingRequests
				.Include(r => r.Animal)
				.FirstOrDefaultAsync(r => r.Id == id);

			if (request == null)
			{
				return null;
			}

			return new CaretakingRequestViewModel
			{
				RequestId = request.Id,
				AnimalId = request.AnimalId,
				AnimalGiverId = request.Animal.GiverId,
				AnimalName = request.Animal.Name,
				AnimalImageUrl = request.Animal.ImageUrl,
				Status = request.IsApprovedForCaretaking ? "Approved" : "Pending",
				SubmittedOn = request.StartDate,
				Duration = request.DurationMonths,
				Message = request.Message,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				PhoneNumber = request.PhoneNumber
			};
		}


		//For Admin Panel
		public async Task<IEnumerable<CaretakingRequestViewModel>> GetAllAsync()
		{
			return await dbContext.CaretakingRequests
				.Include(r => r.Animal)
				.Select(r => new CaretakingRequestViewModel
				{
					RequestId = r.Id,
					AnimalId = r.AnimalId,
					AnimalName = r.Animal.Name,
					AnimalImageUrl = r.Animal.ImageUrl,
					Status = r.IsApprovedForCaretaking ? "Approved" : "Declined",
					SubmittedOn = r.StartDate,
					Duration = r.DurationMonths,
					FirstName = r.FirstName,
					LastName = r.LastName,
					Email = r.Email,
					PhoneNumber = r.PhoneNumber,
					Message = r.Message
				})
				.ToListAsync();
		}


	}
}
