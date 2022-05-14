namespace AppFramework.ViewModels
{
    using Prism.Regions;
    using System.Threading.Tasks;
    using Prism.Ioc;
    using AppFramework.Services;
    using Prism.Services.Dialogs;
    using System.Collections.ObjectModel;
    using AppFramework.Common.Services.Permission;
    using AppFramework.Common;
    using Prism.Commands;
    using AppFramework.Common.ViewModels;
    using System;
    using AppFramework.WindowHost;
    using AppFramework.Views;
    using System.Linq;

    public class NavigationCurdViewModel<T> : ViewModelBase, INavigationAware where T : class
    {
        public NavigationCurdViewModel()
        {
            dialog = ContainerLocator.Container.Resolve<IHostDialogService>();
            regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            permissionService = ContainerLocator.Container.Resolve<IPermissionService>();

            AddCommand = new DelegateCommand(Add);
            ExecuteCommand = new DelegateCommand<string>(Execute);
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());

            gridModelList = new ObservableCollection<T>();
            permissions = new ObservableCollection<PermissionButton>();
        }

        #region 字段/属性

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        private T selectedItem;
        private ObservableCollection<T> gridModelList;
        private ObservableCollection<PermissionButton> permissions;

        public readonly IRegionManager regionManager;
        public readonly IHostDialogService dialog;
        private readonly IPermissionService permissionService;

        public ObservableCollection<PermissionButton> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        public T SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<T> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 添加/编辑/刷新

        private void Execute(string key)
        {
            var permissionButton = Permissions.FirstOrDefault(t => t.Key.Equals(key));
            if (permissionButton != null)
                permissionButton.Ation?.Invoke();
        }

        public async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public async void Edit()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", SelectedItem);

            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"), param);
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public virtual async Task RefreshAsync() => await Task.CompletedTask;

        #endregion

        #region 导航接口

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            /*
             * 当导航发生时,释放当前窗口资源
             */

            GridModelList.Clear();
        }

        public virtual async void OnNavigatedTo(NavigationContext navigationContext)
        {
            CreatePermissions();

            await RefreshAsync();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        #endregion

        #region 公共方法

        protected virtual string GetPageName(string methodName)
        {
            return typeof(T)
                .Name
                .Replace("List", "")
                .Replace("Model", $"{methodName}View");
        }

        public virtual PermissionButton[] CreatePermissionButtons() => new PermissionButton[0];

        private void CreatePermissions()
        {
            if (Permissions == null)
                Permissions = new ObservableCollection<PermissionButton>();
            Permissions.Clear();

            var permissionButtons = CreatePermissionButtons();
            if (permissionButtons != null)
            {
                foreach (var item in permissionButtons)
                {
                    if (permissionService.HasPermission(item.Key))
                        Permissions.Add(item);
                }
            }
        }

        public override async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
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
