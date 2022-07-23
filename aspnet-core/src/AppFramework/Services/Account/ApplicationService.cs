using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Navigation;
using AppFramework.Common.Services.Permission;
using AppFramework.Common.ViewModels;
using AppFramework.Dto;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.Services.Account
{
    public class ApplicationService : ViewModelBase, IApplicationService
    {
        public ApplicationService(
            IHostDialogService dialog,
            IDialogService dialogService,
            IRegionManager regionManager,
            IAccountService accountService,
            INavigationMenuService navigationItemService,
            IProfileAppService profileAppService,
            IApplicationContext applicationContext,
            ProxyProfileControllerService profileControllerService)
        {
            this.dialog = dialog;
            this.accountService = accountService;
            this.applicationContext = applicationContext;
            this.dialogService = dialogService;
            this.navigationItemService = navigationItemService;
            this.regionManager = regionManager;
            this.profileAppService = profileAppService;
            this.profileControllerService = profileControllerService;

            navigationItems = new ObservableCollection<NavigationItem>();
        }

        #region 字段/属性

        private readonly IApplicationContext applicationContext;
        private readonly IDialogService dialogService;
        private readonly IHostDialogService dialog;
        private readonly INavigationMenuService navigationItemService;
        private readonly IRegionManager regionManager;
        private readonly IAccountService accountService;
        private readonly IProfileAppService profileAppService;
        private readonly ProxyProfileControllerService profileControllerService;

        private byte[] photo;
        private byte[] profilePictureBytes;
        private string userNameAndSurname;
        private string emailAddress;
        private string applicationInfo;

        private string applicationName;

        public string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; RaisePropertyChanged(); }
        }

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

        public string EmailAddress
        {
            get => emailAddress;
            set { emailAddress = value; RaisePropertyChanged(); }
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

        private ObservableCollection<PermissionItem> userMenuItems;

        public ObservableCollection<PermissionItem> UserMenuItems
        {
            get { return userMenuItems; }
            set { userMenuItems = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 用户方法

        public async Task ShowMyProfile()
        {
            dialogService.Show(AppViewManager.MyProfile);
            await Task.CompletedTask;
        }

        protected async Task GetUserPhoto()
        {
            await WebRequest.Execute(async () =>
             await profileAppService.GetProfilePictureByUser(applicationContext.LoginInfo.User.Id),
                 async result => await GetProfilePictureByUserSuccessed(result));
        }

        private async Task GetProfilePictureByUserSuccessed(GetProfilePictureOutput output)
        {
            Photo = Convert.FromBase64String(output.ProfilePicture);

            await Task.CompletedTask;
        }

        private async Task SaveProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () => await UpdateProfilePhoto(photoAsBytes, fileName), () =>
                {
                    Photo = photoAsBytes;
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

        public void ChangeProfilePhoto() { }

        public async Task ShowProfilePhoto()
        {
            if (profilePictureBytes == null) return;

            NavigationParameters param = new NavigationParameters();
            param.Add("Value", profilePictureBytes);
            regionManager.Regions[AppRegionManager.Main].RequestNavigate(AppViewManager.ProfilePicture, param);

            await Task.CompletedTask;
        }

        #endregion

        #region 用户菜单方法

        private bool isShowUserPanel;

        public bool IsShowUserPanel
        {
            get { return isShowUserPanel; }
            set { isShowUserPanel = value; RaisePropertyChanged(); }
        }

        public async Task GetApplicationInfo()
        {
            await GetUserPhoto();

            ApplicationName = Local.Localize("EmailActivation_Title");

            UserNameAndSurname = applicationContext.LoginInfo.User.Name;
            EmailAddress = applicationContext.LoginInfo.User.EmailAddress;

            RefreshAuthMenus();

            ApplicationInfo = $"{ApplicationName}\n" +
                            $"v{applicationContext.LoginInfo.Application.Version} " +
                            $"[{applicationContext.LoginInfo.Application.ReleaseDate:yyyyMMdd}]";
        }

        public void RefreshAuthMenus()
        {
            var permissions = applicationContext.Configuration.Auth.GrantedPermissions;
            NavigationItems = navigationItemService.GetAuthMenus(permissions);
            UserMenuItems = new ObservableCollection<PermissionItem>()
            {
               new PermissionItem("accounts",Local.Localize("ManageLinkedAccounts"), "",ManageLinkedAccounts),
               new PermissionItem("manageuser",Local.Localize("ManageUserDelegations"),"",ManageUserDelegations),
               new PermissionItem("password",Local.Localize("ChangePassword"),"",ChangePassword),
               new PermissionItem("loginattempts",Local.Localize("LoginAttempts"),"",LoginAttempts),
               new PermissionItem("picture",Local.Localize("ChangeProfilePicture"),"",ChangeProfilePicture),
               new PermissionItem("mysettings",Local.Localize("MySettings"),"",MySettings),
               new PermissionItem("download",Local.Localize("Download"),"",Download),
               new PermissionItem("logout",Local.Localize("Logout"),"",LogOut),
            };
        }

        public void ExecuteUserAction(string key)
        {
            var item = UserMenuItems.FirstOrDefault(t => t.Key.Equals(key));
            if (item != null) item.Action?.Invoke();
        }

        private async void LogOut()
        {
            if (await dialog.Question(Local.Localize("AreYouSure")))
            {
                IsShowUserPanel = false;
                regionManager.Regions[AppRegionManager.Main].RemoveAll();
                await accountService.LogoutAsync();
            }
        }

        private void ManageLinkedAccounts()
        {

        }

        private void ManageUserDelegations()
        {

        }

        private async void ChangePassword()
        {
            await dialog.ShowDialogAsync(AppViewManager.ChangePassword);
        }

        private void LoginAttempts()
        {

        }

        private void MySettings()
        {

        }

        private void Download()
        {

        }

        private async void ChangeProfilePicture()
        {
            var dialogResult = await dialog.ShowDialogAsync(AppViewManager.ChangeAvatar);

            if (dialogResult.Result == ButtonResult.OK)
            {
                Photo = dialogResult.Parameters.GetValue<byte[]>("Value");
            }
        }

        #endregion
    }
}
