using AppFramework.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.Common.Services.Navigation
{
    public interface INavigationMenuService
    {
        ObservableCollection<NavigationItem> GetAuthMenus(Dictionary<string, string> permissions); 
    }
}