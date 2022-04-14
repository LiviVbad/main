using AppFramework.Application.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFrameworkDemo.Shared.Services.Navigation
{
    public interface INavigationMenuService
    {
        ObservableCollection<NavigationItem> GetAuthMenus(Dictionary<string, string> grantedPermissions);
    }
}