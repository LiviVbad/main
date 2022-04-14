using AppFramework.Shared.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.Shared.Common.Services.Navigation
{
    public interface INavigationMenuService
    {
        ObservableCollection<NavigationItem> GetAuthMenus(Dictionary<string, string> grantedPermissions);
    }
}