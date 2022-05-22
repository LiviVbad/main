namespace AppFramework.ViewModels
{
    using Prism.Regions;
    using System.Threading.Tasks;
    using Prism.Ioc;
    using AppFramework.Services;
    using Prism.Services.Dialogs; 
    using AppFramework.Common.Services.Permission;
    using AppFramework.Common;
    using Prism.Commands;
    using AppFramework.Common.ViewModels;
    using System;
    using AppFramework.WindowHost;
    using AppFramework.Views;

    public class NavigationCurdViewModel : ViewModelBase, INavigationAware
    {
        public NavigationCurdViewModel()
        {
            dialog = ContainerLocator.Container.Resolve<IHostDialogService>();
            dataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
            proxyService = ContainerLocator.Container.Resolve<IPermissionPorxyService>();

            AddCommand = new DelegateCommand(Add);
            ExecuteCommand = new DelegateCommand<string>(proxyService.Execute);
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
        }

        #region 字段/属性

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public readonly IHostDialogService dialog;
        public IDataPagerService dataPager { get; private set; }
        public IPermissionPorxyService proxyService { get; private set; }

        #endregion

        #region 导航数据接口
         
        public async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public async void Edit()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", dataPager.SelectedItem);

            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"), param);
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        private string GetPageName(string methodName) => this.GetType().Name.Replace("ViewModel", $"{methodName}View");

        #endregion

        #region 导航接口

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            /*
             * 当导航发生时,释放当前窗口资源
             */

            dataPager.GridModelList.Clear();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            proxyService.Generate(GetDefaultPermissionItems());
            await OnNavigatedToAsync(navigationContext);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 创建模块具备的默认权限选项清单
        /// </summary>
        /// <returns></returns>
        public virtual PermissionItem[] GetDefaultPermissionItems() => new PermissionItem[0];

        public virtual async Task RefreshAsync() => await Task.CompletedTask;

        public virtual async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await RefreshAsync();
        }

        public override async Task SetBusyAsync(Func<Task> func, string loadingMessage = "")
        {
            IsBusy = true;
            try
            {
                _ = DialogHost.Show(new BusyView(), AppCommonConsts.RootIdentifier);
                await func();
            }
            finally
            {
                DialogHost.Close(AppCommonConsts.RootIdentifier);
                IsBusy = false;
            }
        }

        #endregion 
    }
}
