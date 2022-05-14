﻿using AppFramework.Common.ViewModels;
using Prism.Regions.Navigation;

namespace AppFramework.Shared.ViewModels
{
    public class RegionViewModel : ViewModelBase, IRegionAware
    {
        public virtual bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(INavigationContext navigationContext)
        { }

        public virtual void OnNavigatedTo(INavigationContext navigationContext)
        { }
    }
}