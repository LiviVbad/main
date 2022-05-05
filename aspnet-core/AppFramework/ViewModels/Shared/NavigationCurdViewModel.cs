namespace AppFramework.ViewModels
{
    using AppFramework.Common.ViewModels;
    using Prism.Regions;
    using System.Threading.Tasks;
    using Prism.Ioc;
    using AppFramework.Services;
    using Prism.Services.Dialogs;
    using System.Collections.ObjectModel;
    using AppFramework.Common.Services.Permission;
    using AppFramework.Common;

    public class NavigationCurdViewModel<T> : CurdViewModel<T>, INavigationAware where T : class
    {
        public NavigationCurdViewModel()
        {
            dialog = ContainerLocator.Container.Resolve<IHostDialogService>();
            regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            permissionService = ContainerLocator.Container.Resolve<IPermissionService>();
        }

        #region 字段/属性

        public readonly IRegionManager regionManager;
        public readonly IHostDialogService dialog;
        private readonly IPermissionService permissionService;

        private ObservableCollection<PermissionButton> permissions;

        public ObservableCollection<PermissionButton> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        public virtual PermissionButton[] CreatePermissionButtons() => new PermissionButton[0];

        #endregion

        #region 添加/编辑/删除/刷新

        public override async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public override async void Edit(T selectedItem)
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public override void Delete(T selectedItem) { }

        public override async Task RefreshAsync() => await Task.CompletedTask;

        #endregion

        #region 导航接口

        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

        public virtual async void OnNavigatedTo(NavigationContext navigationContext)
        {
            CreatePermissions();

            await RefreshAsync();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => false;

        #endregion

        #region 公共方法

        protected virtual string GetPageName(string methodName)
        {
            return typeof(T)
                .Name
                .Replace("List", "")
                .Replace("Model", $"{methodName}View");
        }

        protected virtual void CreatePermissions()
        {
            if (Permissions == null)
            {
                Permissions = new ObservableCollection<PermissionButton>();
            }

            Permissions.Clear();

            var permissionButtons = CreatePermissionButtons();
            foreach (var item in permissionButtons)
            {
                if (permissionService.HasPermission(item.Key))
                    Permissions.Add(item);
            }
        }

        #endregion
    }
}
