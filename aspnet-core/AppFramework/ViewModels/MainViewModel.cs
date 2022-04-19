﻿using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Navigation;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace AppFramework.ViewModels
{
    public class MainViewModel : NavigationViewModel
    {
        #region 字段/属性

        public IRegionNavigationJournal journal;
        private readonly IRegionManager regionManager;
        private readonly IProfileAppService profileAppService;
        private readonly IApplicationContext applicationContext;
        private readonly INavigationMenuService navigationItemService;
        private readonly ProxyProfileControllerService profileControllerService;

        private BitmapImage _photo;
        public byte[] profilePictureBytes;
        private string userNameAndSurname;
        private string applicationInfo;
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

        public string ApplicationInfo
        {
            get => applicationInfo;
            set
            {
                applicationInfo = value;
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

        public MainViewModel(
            IRegionManager regionManager,
            IRegionNavigationJournal journal,
            IProfileAppService profileAppService,
            IApplicationContext applicationContext,
            INavigationMenuService navigationItemService,
            ProxyProfileControllerService profileControllerService)
        {
            this.journal = journal;
            this.regionManager = regionManager;
            this.profileAppService = profileAppService;
            this.profileControllerService = profileControllerService;
            this.applicationContext = applicationContext;
            this.navigationItemService = navigationItemService;

            ExecuteCommand = new DelegateCommand<string>(Execute);
            NavigationItems = new ObservableCollection<NavigationItem>();
        }

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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationItems = navigationItemService.GetAuthMenus(applicationContext.Configuration.Auth.GrantedPermissions);

            Navigate(AppViewManager.Dashboard);
        }
    }
}