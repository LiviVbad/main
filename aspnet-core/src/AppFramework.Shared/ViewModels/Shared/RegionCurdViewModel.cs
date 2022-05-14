namespace AppFramework.Shared.ViewModels
{
    using AppFramework.Common.Services.Layer;
    using AppFramework.Common.ViewModels;
    using Prism.Commands;
    using Prism.Ioc;
    using Prism.Navigation;
    using Prism.Regions.Navigation;
    using Prism.Services.Dialogs;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    /// <summary>
    /// Curd Binding的页面导航
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RegionCurdViewModel<T> : ViewModelBase, IRegionAware where T : class
    {
        public RegionCurdViewModel()
        {
            applayer = ContainerLocator.Container.Resolve<IApplayerService>();
            dialogService = ContainerLocator.Container.Resolve<IDialogService>();
            navigationService = ContainerLocator.Container.Resolve<INavigationService>();

            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand<T>(Edit);
            DeleteCommand = new DelegateCommand<T>(Delete);
            LoadMoreCommand = new DelegateCommand(LoadMore);

            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
            GridModelList = new ObservableCollection<T>();
        }

        #region 字段/属性

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<T> EditCommand { get; private set; }
        public DelegateCommand<T> DeleteCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand LoadMoreCommand { get; private set; }

        private readonly IApplayerService applayer;
        public readonly IDialogService dialogService;
        public readonly INavigationService navigationService;

        private ObservableCollection<T> gridModelList;

        public ObservableCollection<T> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        #endregion

        #region 添加/编辑/删除/刷新/加载更多

        public virtual async void Add()
        {
            await navigationService.NavigateAsync(GetPageName("Details"));
        }

        public virtual async void Edit(T selectedItem)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Value", selectedItem);

            await navigationService.NavigateAsync(GetPageName("Details"), param);
        }

        public virtual void LoadMore() { }

        public virtual void Delete(T selectedItem) { }

        public virtual async Task RefreshAsync()
        {
            await Task.CompletedTask;
        }

        public string GetPageName(string methodName)
        {
            return typeof(T)
                .Name
                .Replace("List", "")
                .Replace("Model", $"{methodName}View");
        }

        public override async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            try
            {
                applayer.Show(loadingMessage);
                await func();
            }
            finally
            {
                IsBusy = false;
                applayer.Hide();
            }
        }

        #endregion ICurdAware

        #region 导航方法

        public virtual bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(INavigationContext navigationContext)
        { }

        public virtual async void OnNavigatedTo(INavigationContext navigationContext)
        {
            await RefreshAsync();
        }

        #endregion
    }
}