using Pawsome_Pets.Views.Adoption;
using Pawsome_Pets.Views.CaretakingRequest;

namespace Pawsome_Pets.Views.Profile
{
	public class AdminPanelViewModel
	{
		public IEnumerable<AdoptionRequestViewModel> AdoptionRequests { get; set; } = new List<AdoptionRequestViewModel>();
		public IEnumerable<CaretakingRequestViewModel> CaretakingRequests { get; set; } = new List<CaretakingRequestViewModel>();
		public List<AdminUserViewModel> Users { get; set; } = new List<AdminUserViewModel>();

		public string AdoptionFilterStatus { get; set; }
		public string CaretakingFilterStatus { get; set; }
		public string UserRoleFilter { get; set; }
	}
}
