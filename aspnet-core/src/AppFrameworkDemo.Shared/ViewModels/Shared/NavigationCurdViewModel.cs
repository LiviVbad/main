namespace AppFrameworkDemo.Shared.ViewModels
{
    using AppFrameworkDemo.Shared;
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
    public class NavigationCurdViewModel<T> : RegionViewModel, ICurdViewModel<T>
        where T : class
    {
        public NavigationCurdViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand<T>(Edit);
            DeleteCommand = new DelegateCommand<T>(Delete);
            ExecuteCommand = new DelegateCommand<string>(Execute);
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());

            GridModelList = new ObservableCollection<T>();
            dialogService = ContainerLocator.Container.Resolve<IDialogService>();
            navigationService = ContainerLocator.Container.Resolve<INavigationService>();
        }

        public readonly IDialogService dialogService;
        public readonly INavigationService navigationService;

        #region ICurdAware

        private ObservableCollection<T> gridModelList;

        public ObservableCollection<T> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<T> EditCommand { get; private set; }
        public DelegateCommand<T> DeleteCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public virtual void Execute(string cmd) { }

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

        public virtual void Delete(T selectedItem) { }

        public virtual Task RefreshAsync()
        {
            return Task.CompletedTask;
        }

        public virtual string GetPageName(string methodName)
        {
            return typeof(T)
                .Name
                .Replace("List", "")
                .Replace("Model", $"{methodName}View");
        }

        #endregion ICurdAware

        public override async void OnNavigatedTo(INavigationContext navigationContext)
        { 
            await RefreshAsync();
        }
    }
}