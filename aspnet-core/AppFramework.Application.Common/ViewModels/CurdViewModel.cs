namespace AppFramework.Common.ViewModels
{ 
    using Prism.Commands; 
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public abstract class CurdViewModel<T> : ViewModelBase where T : class
    {
        public CurdViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand<T>(Edit);
            DeleteCommand = new DelegateCommand<T>(Delete);
           
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
            GridModelList = new ObservableCollection<T>();
        }

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
         
        public abstract void Add();

        public abstract void Edit(T selectedItem);

        public abstract void Delete(T selectedItem);

        public abstract Task RefreshAsync();
         
        #endregion ICurdAware 
    }
}