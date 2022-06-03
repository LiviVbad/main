namespace AppFramework.Shared.ViewModels
{ 
    using Prism.Commands; 
    using Prism.Navigation;  
    using System.Collections.ObjectModel; 

    public class RegionCurdViewModel : RegionViewModel
    {
        public RegionCurdViewModel()
        {  
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand<object>(Edit);
            DeleteCommand = new DelegateCommand<object>(Delete);
            LoadMoreCommand = new DelegateCommand(LoadMore);

            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
            GridModelList = new ObservableCollection<object>();
        }

        #region 字段/属性

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<object> EditCommand { get; private set; }
        public DelegateCommand<object> DeleteCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand LoadMoreCommand { get; private set; }
          
        private ObservableCollection<object> gridModelList;

        public ObservableCollection<object> GridModelList
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

        public virtual async void Add() => await navigationService.NavigateAsync(GetPageName("Details"));

        public virtual async void Edit(object selectedItem)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Value", selectedItem);

            await navigationService.NavigateAsync(GetPageName("Details"), param);
        }

        public virtual void LoadMore() { }

        public virtual void Delete(object selectedItem) { }
         
        public string GetPageName(string methodName) => this.GetType().Name.Replace("ViewModel", $"{methodName}View");
    }
}