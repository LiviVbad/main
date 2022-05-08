using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Navigation;
using AppFramework.Models;
using AppFramework.Services;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace AppFramework.ViewModels
{
    public class MainViewModel : NavigationViewModel
    {
        public MainViewModel(
            IThemeService themeService,
            IRegionManager regionManager,
            IRegionNavigationJournal journal,
            IProfileAppService profileAppService,
            IApplicationContext applicationContext,
            INavigationMenuService navigationItemService,
            ProxyProfileControllerService profileControllerService)
        {
            this.journal = journal;
            this.themeService = themeService;
            this.regionManager = regionManager;
            this.profileAppService = profileAppService;
            this.profileControllerService = profileControllerService;
            this.applicationContext = applicationContext;
            this.navigationItemService = navigationItemService;

            ExecuteCommand = new DelegateCommand<string>(Execute);
            ThemeChangeCommand = new DelegateCommand<ThemeItem>(ThemeChanged);
            ThemeModeChangeCommand = new DelegateCommand(ThemeModeChanged);
            NavigationItems = new ObservableCollection<NavigationItem>();
        }

        #region 字段/属性

        public IRegionNavigationJournal journal;
        private readonly IThemeService themeService;
        private readonly IRegionManager regionManager;
        private readonly IProfileAppService profileAppService;
        private readonly IApplicationContext applicationContext;
        private readonly INavigationMenuService navigationItemService;
        private readonly ProxyProfileControllerService profileControllerService;

        private BitmapImage _photo;
        public byte[] profilePictureBytes;
        private string userNameAndSurname;
        private string title;
        public string ApplicationName { get; set; } = "AppFramework";
        private ObservableCollection<NavigationItem> navigationItems;

        public string UserNameAndSurname
        {
            get => userNameAndSurname;
            set
            {
                userNameAndSurname = value;
                RaisePropertyChanged();
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return navigationItems; }
            set { navigationItems = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #endregion

        #region 方法

        public void Execute(string arg)
        {
            switch (arg)
            {
                case "ChangeProfilePhoto": ChangeProfilePhoto(); break;
                case "ShowProfilePhoto": ShowProfilePhoto(); break;
                case "Home": break;
            }
        }

        private void ChangeProfilePhoto()
        { }

        private void ShowProfilePhoto()
        { }

        #endregion

        #region 页面导航

        public void Navigate(NavigationItem navigationItem)
        {
            if (navigationItem.Items?.Count > 0) return;

            Navigate(navigationItem.PageViewName);
        }

        private void Navigate(string pageName)
        {
            regionManager.Regions[AppRegionManager.Main].RequestNavigate(pageName, back =>
                {
                    if (back.Result != null && (bool)back.Result)
                        journal = back.Context.NavigationService.Journal;
                });
        }

        #endregion Navigation

        #region 主题

        private bool isDarkTheme;

        public bool IsDarkTheme
        {
            get { return isDarkTheme; }
            set { isDarkTheme = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ThemeItem> themeItems;

        public ObservableCollection<ThemeItem> ThemeItems
        {
            get { return themeItems; }
            set { themeItems = value; RaisePropertyChanged(); }
        }
        public DelegateCommand ThemeModeChangeCommand { get; }
        public DelegateCommand<ThemeItem> ThemeChangeCommand { get; }

        private void ThemeChanged(ThemeItem obj)
        {
            AppSettings.Instance.ThemeName = obj.DisplayName;
            themeService.SetTheme(themeService.GetCurrent());
        }

        private void ThemeModeChanged()
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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = Local.Localize("EmailActivation_Title");
            GetThemeConfiguration();
            NavigationItems = navigationItemService.GetAuthMenus(applicationContext.Configuration.Auth.GrantedPermissions);
            Navigate(AppViewManager.Dashboard);
        }
    }
}