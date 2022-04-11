using Prism.Regions;
using Prism.Regions.Navigation;
using System;

namespace AppFrameworkDemo.Shared.Services
{
    /// <summary>
    /// 区域导航服务
    /// </summary>
    public class RegionNavigateService : IRegionNavigateService
    {
        private readonly IRegionManager regionManager;
        private readonly IRegionNavigationJournal journal;

        public RegionNavigateService(IRegionManager regionManager,
            IRegionNavigationJournal journal)
        {
            this.regionManager = regionManager;
            this.journal = journal;
        }

        public void Clear()
        {
            journal.Clear();
        }

        public void GoBack()
        {
            if (journal.CanGoBack) journal.GoBack();
        }

        public void GoForward()
        {
            if (journal.CanGoForward) journal.GoForward();
        }

        public void Navigate(string regionName, string pageName)
        {
            regionManager.RequestNavigate(regionName, pageName);
        }

        public void Navigate(string regionName, string pageName, Action<Prism.Regions.Navigation.IRegionNavigationResult> navigationCallback)
        {
            regionManager.RequestNavigate(regionName, pageName, navigationCallback);
        }
    }
}