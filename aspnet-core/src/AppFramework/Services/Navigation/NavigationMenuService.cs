using AppFramework.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppFramework.Common.Services.Navigation;
using AppFramework.Common;

namespace AppFramework.Services
{
    public class NavigationMenuService : INavigationMenuService
    {
        private ObservableCollection<NavigationItem> GetMenuItems()
        {
            return new ObservableCollection<NavigationItem>()
            {
               new NavigationItem("dashboard","Dashboard", AppViewManager.Dashboard, Permkeys.Administration),
               new NavigationItem("organization","OrganizationUnits",AppViewManager.Organization,Permkeys.OrganizationUnits),
               new NavigationItem("role","Roles",AppViewManager.Role,Permkeys.Roles),
               new NavigationItem("user","Users",AppViewManager.User,Permkeys.Users),
               new NavigationItem("auditlog","AuditLogs",AppViewManager.AuditLog,Permkeys.AuditLogs),
               new NavigationItem("property","DynamicProperties",AppViewManager.DynamicProperty,Permkeys.DynamicProperties),
               new NavigationItem("tenant","Tenants",AppViewManager.Tenant,Permkeys.Tenants),
               new NavigationItem("edition","Editions",AppViewManager.Edition,Permkeys.Editions),
               new NavigationItem("language","Languages",AppViewManager.Language,Permkeys.Languages),
               new NavigationItem("setting", "Settings", AppViewManager.Setting, Permkeys.HostSettings),
               new NavigationItem("setting","DemoUiComponents",AppViewManager.Demo,Permkeys.DemoUiComponents)
            };
        }

        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="grantedPermissions"></param>
        /// <returns></returns>
        public ObservableCollection<NavigationItem> GetAuthMenus(Dictionary<string, string> permissions)
        {
            var authorizedMenuItems = new ObservableCollection<NavigationItem>();
            foreach (var item in GetMenuItems())
            {
                //转换特定地区语言的标题
                item.Title = Local.Localize(item.Title);

                if (string.IsNullOrWhiteSpace(item.RequiredPermissionName) ||
                    (permissions != null && permissions.ContainsKey(item.RequiredPermissionName)))
                {
                    authorizedMenuItems.Add(item);
                }
            }
            return authorizedMenuItems;
        }
    }
}