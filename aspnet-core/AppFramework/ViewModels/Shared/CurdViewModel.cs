namespace AppFramework.ViewModels
{
    using AppFramework.Common.ViewModels;
    using Prism.Commands;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public abstract class CurdViewModel<T> : ViewModelBase where T : class
    {
        public CurdViewModel()
        {
            AddCommand = new DelegateCommand(Add);

            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
            GridModelList = new ObservableCollection<T>();
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

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }

        public abstract void Add();

        public abstract void Edit();

        public abstract void Delete();

        public abstract Task RefreshAsync();
    }
}