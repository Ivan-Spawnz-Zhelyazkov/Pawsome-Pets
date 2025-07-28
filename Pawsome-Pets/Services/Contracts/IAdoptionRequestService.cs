using Pawsome_Pets.Views.Adoption;

namespace Pawsome_Pets.Services.Contracts
{
	public interface IAdoptionRequestService
	{
		Task CreateRequestAsync(AdoptionRequestFormModel model, string userId);

		Task<IEnumerable<AdoptionRequestViewModel>> GetRequestsByUserIdAsync(string userId);
	}
}
