﻿using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Common.Services.Navigation;
using AppFramework.Common.ViewModels;
using AppFramework.Dto;
using AppFramework.Models;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace AppFramework.Services.Account
{
    public class ApplicationService : ViewModelBase, IApplicationService
    {
        private readonly IApplicationContext applicationContext;
        private readonly IDialogService dialogService;
        private readonly INavigationMenuService navigationItemService;
        private readonly IThemeService themeService;
        private readonly IRegionManager regionManager;
        private readonly IProfileAppService profileAppService;
        private readonly ProxyProfileControllerService profileControllerService;

        public ApplicationService(
            IDialogService dialogService,
            IRegionManager regionManager,
            INavigationMenuService navigationItemService,
            IThemeService themeService,
            IProfileAppService profileAppService,
            IApplicationContext applicationContext,
            ProxyProfileControllerService profileControllerService)
        {
            this.applicationContext = applicationContext;
            this.dialogService = dialogService;
            this.navigationItemService = navigationItemService;
            this.themeService = themeService;
            this.regionManager = regionManager;
            this.profileAppService = profileAppService;
            this.profileControllerService = profileControllerService;

            navigationItems = new ObservableCollection<NavigationItem>();
        }

        private byte[] photo;
        private byte[] profilePictureBytes;
        private string userNameAndSurname;
        private string applicationInfo;
        public string ApplicationName { get; set; } = Local.Localize("EmailActivation_Title");

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
            await WebRequest.Execute(async () =>
             await profileAppService.GetProfilePictureByUser(applicationContext.LoginInfo.User.Id),
                 async result => await GetProfilePictureByUserSuccessed(result));
        }

        private async Task GetProfilePictureByUserSuccessed(GetProfilePictureOutput output)
        {
            profilePictureBytes = Convert.FromBase64String(output.ProfilePicture);
            //Photo = ImageSource.FromStream(() => new MemoryStream(profilePictureBytes));

            await Task.CompletedTask;
        }

        private async Task SaveProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () => await UpdateProfilePhoto(photoAsBytes, fileName), () =>
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

        public void ChangeProfilePhoto() { }

        public async Task ShowProfilePhoto()
        {
            if (profilePictureBytes == null) return;

            NavigationParameters param = new NavigationParameters();
            param.Add("Value", profilePictureBytes);
            regionManager.Regions[AppRegionManager.Main].RequestNavigate(AppViewManager.ProfilePicture, param);

            await Task.CompletedTask;
        }

        public async Task GetApplicationInfo()
        {
            await GetUserPhoto();

            GetThemeConfiguration();

            UserNameAndSurname = applicationContext.LoginInfo.User.Name + " " + applicationContext.LoginInfo.User.Surname;

            var permissions = applicationContext.Configuration.Auth.GrantedPermissions;
            NavigationItems = navigationItemService.GetAuthMenus(permissions);

            ApplicationInfo = $"{ApplicationName}\n" +
                              $"v{applicationContext.LoginInfo.Application.Version} " +
                              $"[{applicationContext.LoginInfo.Application.ReleaseDate:yyyyMMdd}]";
        }

        #region 主题

        private bool isDarkTheme;
        private ObservableCollection<ThemeItem> themeItems;

        public bool IsDarkTheme
        {
            get { return isDarkTheme; }
            set { isDarkTheme = value; RaisePropertyChanged(); }
        }
         
        public ObservableCollection<ThemeItem> ThemeItems
        {
            get { return themeItems; }
            set { themeItems = value; RaisePropertyChanged(); }
        }

        public void SetTheme(string displayName)
        {
            AppSettings.Instance.ThemeName = displayName;
            themeService.SetTheme(themeService.GetCurrent());
        }

        public void SetThemeMode()
        {
            IsDarkTheme = !IsDarkTheme;
            AppSettings.Instance.IsDarkTheme = IsDarkTheme;
            themeService.SetTheme(themeService.GetCurrent());
        }

        private void GetThemeConfiguration()
        {
            ThemeItems = themeService.GetThemes();
            IsDarkTheme = AppSettings.Instance.IsDarkTheme;
        }

        #endregion
    }
}
