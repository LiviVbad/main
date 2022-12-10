using AppFramework.Admin;
using AppFramework.Shared;
using Prism.Mvvm;
using Prism.Regions;  
using System.Linq;
using System.Windows.Controls;

namespace AppFramework.Admin.Services
{
    public class NavigationService : BindableBase
    {
        private readonly IRegionManager regionManager;
        private IRegion NavigationRegion => regionManager.Regions[AppRegions.Main];

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        public NavigationService(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Navigate(string pageName, NavigationParameters navigationParameters = null)
        {
            if (string.IsNullOrWhiteSpace(pageName)) return;

            var view = NavigationRegion.Views.FirstOrDefault(q => q.GetType().Name.Equals(pageName));
            if (view == null)
            {
                NavigationRegion.RequestNavigate(pageName, NavigateionCallBack, navigationParameters);
            } 
            else
            {
                SelectedIndex = NavigationRegion.Views.IndexOf(view);
            }
        }

        public void RemoveView(object view)
        {
            if (NavigationRegion.Views.Contains(view))
            {
                /*
                 * 关闭Tab后调用OnNavigatedFrom，如需手动释放资源请在 OnNavigatedFrom 中处理
                 */
                if (view is UserControl viewControl && viewControl.DataContext is INavigationAware navigationAware)
                    navigationAware.OnNavigatedFrom(null);

                NavigationRegion.Remove(view);
            }
        }

        private void NavigateionCallBack(NavigationResult navigationResult)
        {
            if (navigationResult.Result != null && !(bool)navigationResult.Result)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(navigationResult.Error.Message);
#endif
            }
            else
            {
                SelectedIndex = NavigationRegion.Views.Count() - 1;
            }
        }
    }
}
