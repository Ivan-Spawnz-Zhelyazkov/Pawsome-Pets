using Pawsome_Pets.Views.Adoption;

namespace Pawsome_Pets.Services.Contracts
{
	public interface IAdoptionRequestService
	{
		Task CreateRequestAsync(AdoptionRequestFormModel model, string userId);
		Task<IEnumerable<AdoptionRequestFormModel>> GetRequestsByUserIdAsync(string userId);
	}
}
