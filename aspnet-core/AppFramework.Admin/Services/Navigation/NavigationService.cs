using AppFramework.Shared;
using Prism.Mvvm;
using Prism.Regions;
using Syncfusion.Data.Extensions;
using System;
using System.Linq;

namespace AppFramework.Services
{
    public class NavigationService : BindableBase
    {
        private readonly IRegionManager regionManager;
        private IRegion NavigationRegion => regionManager.Regions[AppRegionManager.Main];

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
                NavigationRegion.RequestNavigate(pageName, NavigateionCallBack, navigationParameters);
            else
            {
                SelectedIndex = NavigationRegion.Views.IndexOf(view);
            }
        }

        public void RemoveView(object view)
        {
            if (NavigationRegion.Views.Contains(view))
                NavigationRegion.Remove(view);
        }

        private void NavigateionCallBack(NavigationResult navigationResult)
        {
            if (navigationResult.Result != null && !(bool)navigationResult.Result)
            { }
        }
    }
}
