using Prism.Commands;
using Prism.Events; 

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
