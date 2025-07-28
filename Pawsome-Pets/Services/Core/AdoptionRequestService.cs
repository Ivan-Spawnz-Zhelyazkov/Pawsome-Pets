using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pawsome_Pets.Data;
using Pawsome_Pets.Models;
using Pawsome_Pets.Services.Contracts;
using Pawsome_Pets.Views.Adoption;

namespace Pawsome_Pets.Services.Core
{
	public class AdoptionRequestService : IAdoptionRequestService
	{
		private readonly PawsomeDbContext dbContext;

		public AdoptionRequestService(PawsomeDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task CreateRequestAsync(AdoptionRequestFormModel model, string userId)
		{
			AdoptionRequest adoptionRequest = new AdoptionRequest
			{
				AnimalId = model.AnimalId,
				AdopterId = userId,
				FullName = model.FullName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				Message = model.Message,
				Status = "Pending",
				CreatedOn = DateTime.UtcNow
			};

			dbContext.AdoptionRequests.Add(adoptionRequest);
			await dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<AdoptionRequestViewModel>> GetRequestsByUserIdAsync(string userId)
		{
			return await dbContext.AdoptionRequests
				.Where(r => r.AdopterId == userId)
				.Select(r => new AdoptionRequestViewModel
				{
					Id = r.Id,
					FullName = r.FullName,
					Email = r.Email,
					PhoneNumber = r.PhoneNumber,
					Message = r.Message,
					AnimalName = r.Animal.Name,
					AnimalImageUrl = r.Animal.ImageUrl,
					Status = r.Status,
					SubmittedOn = r.CreatedOn
				})
				.ToListAsync();
		}
	}
}