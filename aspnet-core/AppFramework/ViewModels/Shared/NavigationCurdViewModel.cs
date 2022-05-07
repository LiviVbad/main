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

            GridModelList = new ObservableCollection<T>();
        }

        #region 字段/属性

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public readonly IRegionManager regionManager;
        public readonly IHostDialogService dialog;
        private readonly IPermissionService permissionService;

        private ObservableCollection<PermissionButton> permissions;

        public ObservableCollection<PermissionButton> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        private T selectedItem;

        public T SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T> gridModelList;

        public ObservableCollection<T> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        public virtual PermissionButton[] CreatePermissionButtons() => new PermissionButton[0];

        #endregion

        #region 添加/编辑/删除/刷新

        public virtual void Execute(string obj) { }

        public virtual async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public virtual async void Edit()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public virtual void Delete() { }

        public virtual async Task RefreshAsync() => await Task.CompletedTask;

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
