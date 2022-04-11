using AppFrameworkDemo.Shared.Services;
using Prism.Commands;
using Prism.Navigation;
using System.IO;
using Xamarin.Forms;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class ProfilePictureViewModel : NavigationViewModel
    {
        private ImageSource photo;
        private readonly IApplicationService appService;

        public ImageSource Photo
        {
            get => photo;
            set { photo = value; RaisePropertyChanged(); }
        }

        public DelegateCommand ChangeProfilePictureCommand { get; private set; }

        public ProfilePictureViewModel(IApplicationService appService)
        {
            ChangeProfilePictureCommand=new DelegateCommand(ChangeProfilePicture);
            this.appService=appService;
        }

        private void ChangeProfilePicture()
        {
            appService.ChangeProfilePhoto();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var profilePictureBytes = parameters.GetValue<byte[]>("Value");
            Photo = ImageSource.FromStream(() => new MemoryStream(profilePictureBytes));
        }
    }
}