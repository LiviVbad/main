using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging; 
using AppFramework.Authorization.Users.Profile;
using AppFramework.ApiClient;
using AppFramework.Common.Services.Navigation;
using AppFramework.Common.Models;
using AppFramework.Common.Core;

namespace AppFramework.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private IRegionNavigationJournal journal;

        public readonly IProfileAppService profileAppService;
        public readonly ProxyProfileControllerService profileControllerService;
        public readonly IApplicationContext applicationContext;
        public readonly INavigationMenuService navigationItemService;

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

        public MainViewModel(
            IContainerProvider provider,
            IEventAggregator aggregator,
            IRegionManager regionManager,
            IRegionNavigationJournal journal)
        {
            profileAppService = provider.Resolve<IProfileAppService>();
            profileControllerService = provider.Resolve<ProxyProfileControllerService>();
            applicationContext = provider.Resolve<IApplicationContext>();
            navigationItemService = provider.Resolve<INavigationMenuService>();
            NavigationItems = new ObservableCollection<NavigationItem>();
            ExecuteCommand = new DelegateCommand<string>(Execute);

            this.journal = journal;
            this.regionManager = regionManager;
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

        void PageAppearing() { }

        void ChangeProfilePhoto() { }

        void ShowProfilePhoto() { }

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

        void Navigate(string pageName, NavigationParameters param = null)
        {
            regionManager
                .Regions[AppRegionManager.Main]
                .RequestNavigate(pageName, back =>
                {
                    if ((bool)back.Result)
                        journal = back.Context.NavigationService.Journal;
                }, param);
        }

        #endregion
    }
}
