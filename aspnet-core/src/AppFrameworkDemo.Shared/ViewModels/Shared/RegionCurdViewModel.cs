namespace AppFramework.Shared.ViewModels
{
    using AppFramework.Common.Services.Layer; 
    using Prism.Commands;
    using Prism.Ioc;
    using Prism.Navigation;
    using Prism.Regions.Navigation;
    using Prism.Services.Dialogs;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Curd Binding的页面导航
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RegionCurdViewModel<T> : CurdViewModel<T>, IRegionAware where T : class
    {
        public RegionCurdViewModel()
        {
            LoadMoreCommand = new DelegateCommand(LoadMore);
            applayer = ContainerLocator.Container.Resolve<IApplayerService>();
            dialogService = ContainerLocator.Container.Resolve<IDialogService>();
            navigationService = ContainerLocator.Container.Resolve<INavigationService>();
        }

        public DelegateCommand LoadMoreCommand { get; private set; }
        private readonly IApplayerService applayer;
        public readonly IDialogService dialogService;
        public readonly INavigationService navigationService;

        #region ICurdViewModel

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        public async override void Add()
        {
            await navigationService.NavigateAsync(GetPageName("Details"));
        }

        public async override void Edit(T selectedItem)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Value", selectedItem);

            await navigationService.NavigateAsync(GetPageName("Details"), param);
        }

        public virtual void LoadMore() { }

        public override void Delete(T selectedItem) { }

        public override async Task RefreshAsync()
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

        #region IRegionAware

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