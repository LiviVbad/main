namespace AppFramework.ViewModels
{
    using AppFramework.Common.ViewModels;
    using Prism.Regions;
    using System.Threading.Tasks;
    using Prism.Ioc;
    using AppFramework.Services;
    using Prism.Services.Dialogs;

    public class NavigationCurdViewModel<T> : CurdViewModel<T>, INavigationAware where T : class
    {
        public readonly IRegionManager regionManager;
        public readonly IHostDialogService dialog;

        public NavigationCurdViewModel()
        {
            regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            dialog = ContainerLocator.Container.Resolve<IHostDialogService>();
        }

        public override async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
            {
                await RefreshAsync();
            }
        }

        public override async void Edit(T selectedItem)
        {
            var dialogResult = await dialog.ShowDialogAsync(GetPageName("Details"));
            if (dialogResult.Result == ButtonResult.OK)
            {
                await RefreshAsync();
            }
        }

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

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        public virtual async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await RefreshAsync();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
    }
}
