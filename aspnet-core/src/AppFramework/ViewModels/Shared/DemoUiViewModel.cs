using AppFramework.Common;

namespace AppFramework.ViewModels.Shared
{
    public class DemoUiViewModel : NavigationViewModel
    {
        public DemoUiViewModel()
        {
            Title = Local.Localize("DemoUiComponents");
        }
    }
}
