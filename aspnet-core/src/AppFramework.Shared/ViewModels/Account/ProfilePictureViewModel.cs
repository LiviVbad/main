using AppFramework.Common.Services;
using AppFramework.Shared.Services;
using Prism.Commands;
using Prism.Navigation;
using System.IO;
using Xamarin.Forms;

namespace AppFramework.Shared.ViewModels
{
    public class ProfilePictureViewModel : NavigationViewModel
    {
        public IApplicationService appService { get; set; }

        public DelegateCommand ChangeProfilePictureCommand { get; private set; }

        public ProfilePictureViewModel(IApplicationService appService)
        {
            ChangeProfilePictureCommand = new DelegateCommand(ChangeProfilePicture);
            this.appService = appService;
        }

        private void ChangeProfilePicture()
        {
            appService.ChangeProfilePhoto();
        }
    }
}