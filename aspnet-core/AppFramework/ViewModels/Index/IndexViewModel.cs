using Prism.Commands;

namespace AppFramework.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public DelegateCommand<string> NavigateCommand { get; private set; }

        public IndexViewModel()
        {
        }
    }
}