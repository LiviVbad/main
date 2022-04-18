using Abp.IO.Extensions;
using Acr.UserDialogs;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Common.Services.Navigation;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Dto;
using AppFramework.Shared.Controls;
using AppFramework.Shared.Localization;
using FFImageLoading;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppFramework.Common.ViewModels;
using AppFramework.Common;

namespace AppFramework.Shared.Services
{
    public class ApplicationService : ViewModelBase, IApplicationService
    {
        private readonly IApplicationContext applicationContext;
        private readonly IDialogService dialogService;
        private readonly INavigationMenuService navigationItemService;
        private readonly INavigationService navigationService;
        private readonly IProfileAppService profileAppService;
        private readonly ProxyProfileControllerService profileControllerService;

        public ApplicationService(
            IApplicationContext applicationContext,
            IDialogService dialogService,
            INavigationMenuService navigationItemService,
            INavigationService navigationService,
            IProfileAppService profileAppService,
            ProxyProfileControllerService profileControllerService)
        {
            this.applicationContext = applicationContext;
            this.dialogService = dialogService;
            this.navigationItemService = navigationItemService;
            this.navigationService = navigationService;
            this.profileAppService = profileAppService;
            this.profileControllerService = profileControllerService;

            navigationItems = new ObservableCollection<NavigationItem>();
        }

        private byte[] photo;
        private byte[] profilePictureBytes;
        private string userNameAndSurname;
        private string applicationInfo;
        public string ApplicationName { get; set; } = "AppFramework";

        public byte[] Photo
        {
            get => photo;
            set
            {
                photo = value;
                RaisePropertyChanged();
            }
        }

        public string UserNameAndSurname
        {
            get => userNameAndSurname;
            set { userNameAndSurname = value; RaisePropertyChanged(); }
        }

        public string ApplicationInfo
        {
            get => applicationInfo;
            set { applicationInfo = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<NavigationItem> navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return navigationItems; }
            set { navigationItems = value; RaisePropertyChanged(); }
        }

        public async Task ShowMyProfile()
        {
            dialogService.ShowDialog(AppViewManager.MyProfile);

            await Task.CompletedTask;
        }

        protected async Task GetUserPhoto()
        {
            await WebRequestRuner.Execute(async () =>
             await profileAppService.GetProfilePictureByUser(applicationContext.LoginInfo.User.Id),
                 async result => await GetProfilePictureByUserSuccessed(result));
        }

        private async Task GetProfilePictureByUserSuccessed(GetProfilePictureOutput output)
        {
            profilePictureBytes = Convert.FromBase64String(output.ProfilePicture);
            //Photo = ImageSource.FromStream(() => new MemoryStream(profilePictureBytes));

            await Task.CompletedTask;
        }

        private static async Task PickProfilePictureAsync(Func<MediaFile, Task> picturePicked)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
                return;

            using (var photo = await CrossMedia.Current.PickPhotoAsync())
            {
                await picturePicked(photo);
            }
        }

        private async Task TakeProfilePhotoAsync(Func<MediaFile, Task> photoTaken)
        {
            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                AppDialogHelper.Warn("NoCamera");
                return;
            }

            await SetBusyAsync(async () =>
            {
                using (var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Front,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    AllowCropping = true,
                    CompressionQuality = 80,
                    MaxWidthHeight = 700
                }))
                {
                    await photoTaken(photo);
                }
            });
        }

        /// <summary>
        /// Shows a crop view to crop the media file.
        /// Native image cropping feature is available only on UWP and IOS.
        /// For Android devices, custom cropping is implemented.
        /// </summary>
        private async Task CropImageIfNeedsAsync(MediaFile photo)
        {
            if (photo == null) return;

            if (Device.RuntimePlatform == Device.Android)
            {
                var cropModalView = new CropView(photo.Path, OnCropCompleted, Local.Localize("Ok"), Local.Localize("Rotate"), Local.Localize("Cancel"));
                //await ModalService.ShowModalAsync(cropModalView);

                NavigationParameters param = new NavigationParameters();
                param.Add("Value", profilePictureBytes);
                await navigationService.NavigateAsync(AppViewManager.ProfilePicture, param);
            }
            else
            {
                await OnCropCompleted(File.ReadAllBytes(photo.Path), Path.GetFileName(photo.Path));
            }
        }

        private async Task OnCropCompleted(byte[] croppedImageBytes, string fileName)
        {
            if (croppedImageBytes == null)
            {
                return;
            }

            var jpgStream = await ResizeImageAsync(croppedImageBytes);
            await SaveProfilePhoto(jpgStream.GetAllBytes(), fileName);
        }

        private static async Task<Stream> ResizeImageAsync(byte[] imageBytes, int width = 256, int height = 256)
        {
            var result = ImageService.Instance.LoadStream(token =>
            {
                var tcs = new TaskCompletionSource<Stream>();
                tcs.TrySetResult(new MemoryStream(imageBytes));
                return tcs.Task;
            }).DownSample(width, height);

            return await result.AsJPGStreamAsync();
        }

        private async Task SaveProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(async () => await UpdateProfilePhoto(photoAsBytes, fileName), () =>
                {
                    //Photo = ImageSource.FromStream(() => new MemoryStream(photoAsBytes));
                    CloneProfilePicture(photoAsBytes);
                    return Task.CompletedTask;
                });
            });
        }

        private void CloneProfilePicture(byte[] photoAsBytes)
        {
            profilePictureBytes = new byte[photoAsBytes.Length];
            photoAsBytes.CopyTo(profilePictureBytes, 0);
        }

        private async Task UpdateProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            var fileToken = Guid.NewGuid().ToString();

            using (Stream photoStream = new MemoryStream(photoAsBytes))
            {
                await profileControllerService.UploadProfilePicture(content =>
                {
                    content.AddFile("file", photoStream, fileName);
                    content.AddString(nameof(FileDto.FileToken), fileToken);
                    content.AddString(nameof(FileDto.FileName), fileName);
                }).ContinueWith(uploadResult =>
                {
                    if (uploadResult == null)
                        return;

                    profileAppService.UpdateProfilePicture(new UpdateProfilePictureInput
                    {
                        FileToken = fileToken
                    });
                });
            }
        }

        public void ChangeProfilePhoto()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig
            {
                Title = Local.Localize("ChangeProfilePicture"),
                Options = new List<ActionSheetOption>  {
                    new ActionSheetOption(Local.Localize("PickFromGallery"),  async () => await PickProfilePictureAsync(CropImageIfNeedsAsync)),
                    new ActionSheetOption(Local.Localize("TakePhoto"),  async () => await TakeProfilePhotoAsync(CropImageIfNeedsAsync))
                }
            });
        }

        public async Task ShowProfilePhoto()
        {
            if (profilePictureBytes == null) return;

            NavigationParameters param = new NavigationParameters();
            param.Add("Value", profilePictureBytes);
            await navigationService.NavigateAsync(AppViewManager.ProfilePicture, param);
        }

        public async Task GetApplicationInfo()
        {
            await GetUserPhoto();

            UserNameAndSurname = applicationContext.LoginInfo.User.Name + " " + applicationContext.LoginInfo.User.Surname;

            var permissions = applicationContext.Configuration.Auth.GrantedPermissions;
            NavigationItems = navigationItemService.GetAuthMenus(permissions);

            ApplicationInfo = $"{ApplicationName}\n" +
                              $"v{applicationContext.LoginInfo.Application.Version} " +
                              $"[{applicationContext.LoginInfo.Application.ReleaseDate:yyyyMMdd}]";
        }
    }
}