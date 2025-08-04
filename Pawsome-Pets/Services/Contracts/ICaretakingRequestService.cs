using Pawsome_Pets.Models;
using Pawsome_Pets.Views.CaretakingRequest;
using System.Threading.Tasks;

namespace Pawsome_Pets.Services.Contracts
{
	public interface ICaretakingRequestService
	{
		Task CreateRequestAsync(CaretakingRequestFormModel model, string userId);
		Task<IEnumerable<CaretakingRequestViewModel>> GetRequestByUserIdAsync(string userId);
		Task ApproveRequestAsync(int requestId);
		Task DeclineRequestAsync(int requestId);

		Task<IEnumerable<CaretakingRequestViewModel>> GetRequestsToGiverAnimalsAsync(string giverId);
		Task<CaretakingRequestViewModel?> GetRequestByIdAsync(int id);


		//For Admins to view all requests
		Task<IEnumerable<CaretakingRequestViewModel>> GetAllAsync();

	}
}
