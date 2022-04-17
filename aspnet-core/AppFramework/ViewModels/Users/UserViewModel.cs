using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;

namespace AppFramework.ViewModels
{
    public class UserViewModel
    {
        public readonly IProfileAppService profileAppService;

        public GetUsersInput input { get; set; }

        public UserViewModel(IProfileAppService profileAppService)
        {
            input = new GetUsersInput
            {
                Filter = "",
                SkipCount = 0
            };
            this.profileAppService = profileAppService;
        }
    }
}