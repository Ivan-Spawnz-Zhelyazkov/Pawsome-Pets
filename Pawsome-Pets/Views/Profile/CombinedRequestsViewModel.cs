using Pawsome_Pets.Views.Adoption;
using Pawsome_Pets.Views.CaretakingRequest;

namespace Pawsome_Pets.Views.Profile
{
	public class CombinedRequestsViewModel
	{
		public IEnumerable<AdoptionRequestViewModel> AdoptionRequests { get; set; } = new List<AdoptionRequestViewModel>();
		public IEnumerable<CaretakingRequestViewModel> CaretakingRequests { get; set; } = new List<CaretakingRequestViewModel>();
	}
}

