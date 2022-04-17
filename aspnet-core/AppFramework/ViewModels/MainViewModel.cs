using AppFramework.ApiClient;
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
    public class MainViewModel : ViewModelBase
    {
        #region 字段/属性

        public IRegionNavigationJournal journal;
        private readonly IProfileAppService profileAppService;
        private readonly ProxyProfileControllerService profileControllerService;
        private readonly IApplicationContext applicationContext;
        private readonly INavigationMenuService navigationItemService;

        public string ApplicationName { get; set; } = "AppFramework";
        private BitmapImage _photo;
        public byte[] profilePictureBytes;
        private string userNameAndSurname;
        private string applicationInfo;

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

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #endregion

        public MainViewModel(IProfileAppService profileAppService,
            IRegionManager regionManager,
            IRegionNavigationJournal journal,
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

        public void Execute(string arg)
        {
            switch (arg)
            {
                case "PageAppearing": PageAppearing(); break;
                case "ChangeProfilePhoto": ChangeProfilePhoto(); break;
                case "ShowProfilePhoto": ShowProfilePhoto(); break;
                case "Home": break;
            }
        }

        private void PageAppearing()
        { }

        private void ChangeProfilePhoto()
        { }

        private void ShowProfilePhoto()
        { }

        #region Navigation

        private ObservableCollection<NavigationItem> navigationItems;
        private readonly IRegionManager regionManager;

        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return navigationItems; }
            set { navigationItems = value; RaisePropertyChanged(); }
        }

        public void Navigate(NavigationItem navigationItem)
        {
            if (navigationItem.Items?.Count > 0) return;
            NavigationParameters parameter = new NavigationParameters();
            parameter.Add("Value", navigationItem.NavigationParameter);
            Navigate(navigationItem.PageViewName, parameter);
        }

        private void Navigate(string pageName, NavigationParameters param = null)
        {
            regionManager.Regions[AppRegionManager.Main].RequestNavigate(pageName, back =>
                {
                    if ((bool)back.Result)
                    {
                        journal = back.Context.NavigationService.Journal;
                    }
                }, param);
        }

        #endregion Navigation
    }
}