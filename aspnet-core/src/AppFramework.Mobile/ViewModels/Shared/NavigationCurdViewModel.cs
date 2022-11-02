namespace AppFramework.Shared.ViewModels
{
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Ioc;
    using AppFramework.Shared.Services.Permission;
    using AppFramework.Shared.Services.Datapager;
    using AppFramework.Shared.Core;

    public class NavigationCurdViewModel : RegionViewModel
    {
        public NavigationCurdViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            LoadMoreCommand = new DelegateCommand(LoadMore);
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());

            dataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
            proxyCommand = ContainerLocator.Container.Resolve<IPorxyCommandService>();
            proxyCommand.Generate(CreatePermissionItems());

            var messenger = ContainerLocator.Container.Resolve<IMessenger>();
            messenger.Sub(this.GetType().Name, async () => await RefreshAsync());
        }

        public IDataPagerService dataPager { get; private set; }
        public IPorxyCommandService proxyCommand { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand LoadMoreCommand { get; private set; }

        public virtual async void Add() => await navigationService.NavigateAsync(GetPageName("Details"));

        public virtual async void Edit()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Value", dataPager.SelectedItem);

            await navigationService.NavigateAsync(GetPageName("Details"), param);
        }

        public virtual void LoadMore() { }

        public string GetPageName(string methodName) => this.GetType().Name.Replace("ViewModel", $"{methodName}View");

        /// <summary>
        /// 创建模块具备的默认权限选项清单
        /// </summary>
        /// <returns></returns>
        protected virtual PermissionItem[] CreatePermissionItems() => new PermissionItem[0];
    }
}