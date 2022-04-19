using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class NavigationViewModel : ViewModelBase, INavigationAware
    {
        #region INavigationAware

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        { }

        #endregion
    }
}
