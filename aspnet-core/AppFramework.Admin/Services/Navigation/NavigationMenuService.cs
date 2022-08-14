﻿using AppFramework.Shared.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using AppFramework.Shared;  

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
                      new NavigationItem("dashboard","Dashboard", AppViews.Dashboard, AppPermissions.Administration),
                      new NavigationItem("organization","OrganizationUnits",AppViews.Organization,AppPermissions.OrganizationUnits),
                      new NavigationItem("role","Roles",AppViews.Role,AppPermissions.Roles),
                      new NavigationItem("user","Users",AppViews.User,AppPermissions.Users),
                      new NavigationItem("auditlog","AuditLogs",AppViews.AuditLog,AppPermissions.AuditLogs),
                      new NavigationItem("property","DynamicProperties",AppViews.DynamicProperty,AppPermissions.DynamicProperties),
                      new NavigationItem("language","Languages",AppViews.Language,AppPermissions.Languages),
                      new NavigationItem("version", "VersionManager", AppViews.Version, AppPermissions.Administration),
                      new NavigationItem("edition","Editions",AppViews.Edition,AppPermissions.Editions),
               }),

               new NavigationItem("tenant","Tenants",AppViews.Tenant,AppPermissions.Tenants),
               new NavigationItem("visual", "VisualSettings", AppViews.Visual, AppPermissions.Administration),
               new NavigationItem("setting", "Settings", AppViews.Setting, AppPermissions.HostSettings),
               new NavigationItem("demo","DemoUiComponents",AppViews.Demo,AppPermissions.DemoUiComponents)
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