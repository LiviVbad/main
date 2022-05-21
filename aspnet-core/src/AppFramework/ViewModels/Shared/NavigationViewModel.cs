namespace AppFramework.ViewModels
{
    using AppFramework.Common;
    using AppFramework.Common.ViewModels;
    using Prism.Commands;
    using Prism.Regions;
    using Prism.Ioc;
    using System.Threading.Tasks;
    using AppFramework.Common.Services.Permission;

    public class NavigationViewModel : ViewModelBase, INavigationAware
    {
        public NavigationViewModel()
        {
            proxyService = ContainerLocator.Container.Resolve<IPermissionPorxyService>();
            ExecuteCommand = new DelegateCommand<string>(proxyService.Execute);
        }

        public IPermissionPorxyService proxyService { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #region INavigationAware

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            proxyService.Generate(GetDefaultPermissionItems());
            await OnNavigatedToAsync(navigationContext);
        }

        #endregion

        /// <summary>
        /// 创建模块具备的默认权限选项清单
        /// </summary>
        /// <returns></returns>
        public virtual PermissionItem[] GetDefaultPermissionItems() => new PermissionItem[0];

        public virtual async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await RefreshAsync();
        }

        public virtual async Task RefreshAsync() => await Task.CompletedTask;
    }
}
