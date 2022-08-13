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
               new NavigationItem("demo","Administration","",AppPermissions.Administration,new ObservableCollection<NavigationItem>()
               {
                      new NavigationItem("dashboard","Dashboard", AppViewManager.Dashboard, AppPermissions.Administration),
                      new NavigationItem("organization","OrganizationUnits",AppViewManager.Organization,AppPermissions.OrganizationUnits),
                      new NavigationItem("role","Roles",AppViewManager.Role,AppPermissions.Roles),
                      new NavigationItem("user","Users",AppViewManager.User,AppPermissions.Users),
                      new NavigationItem("auditlog","AuditLogs",AppViewManager.AuditLog,AppPermissions.AuditLogs),
                      new NavigationItem("property","DynamicProperties",AppViewManager.DynamicProperty,AppPermissions.DynamicProperties),
                      new NavigationItem("language","Languages",AppViewManager.Language,AppPermissions.Languages),
                      new NavigationItem("version", "VersionManager", AppViewManager.Version, AppPermissions.Administration),
                      new NavigationItem("edition","Editions",AppViewManager.Edition,AppPermissions.Editions),
               }),

               new NavigationItem("tenant","Tenants",AppViewManager.Tenant,AppPermissions.Tenants),
               new NavigationItem("visual", "VisualSettings", AppViewManager.Visual, AppPermissions.Administration),
               new NavigationItem("setting", "Settings", AppViewManager.Setting, AppPermissions.HostSettings),
               new NavigationItem("demo","DemoUiComponents",AppViewManager.Demo,AppPermissions.DemoUiComponents)
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
                    if (item.Items != null)
                    {
                        foreach (var submenuItem in item.Items)
                            submenuItem.Title = Local.Localize(submenuItem.Title);
                    }
                    authorizedMenuItems.Add(item);
                }
            }
            return authorizedMenuItems;
        }
    }
}