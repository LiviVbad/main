﻿using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.Shared.Services
{
    public class NavigationMenuService : INavigationMenuService
    {
        private ObservableCollection<NavigationItem> GetMenuItems()
        {
            return new ObservableCollection<NavigationItem>()
            {
               new NavigationItem("\ue6c4","Dashboard", AppViewManager.Dashboard, PermissionKey.Administration),
               new NavigationItem("\uec07","Administration","",PermissionKey.Administration,new ObservableCollection<NavigationItem>()
               {
                      new NavigationItem("\ue64e","OrganizationUnits",AppViewManager.Organization,PermissionKey.OrganizationUnits),
                      new NavigationItem("\ue787","Roles",AppViewManager.Role,PermissionKey.Roles),
                      new NavigationItem("\ue658","Users",AppViewManager.User,PermissionKey.Users),
                      new NavigationItem("\ue617","AuditLogs",AppViewManager.AuditLog,PermissionKey.AuditLogs),
                      new NavigationItem("\ue634","DynamicProperties",AppViewManager.DynamicProperty,PermissionKey.DynamicProperties),
                      new NavigationItem("\ue635","Tenants",AppViewManager.Tenant,PermissionKey.Tenants),
                      new NavigationItem("\ue657","Editions",AppViewManager.Edition,PermissionKey.Editions),
               }),
               new NavigationItem("\ue62e","Languages",AppViewManager.Language,PermissionKey.Languages),
               new NavigationItem("\ue600","Settings",AppViewManager.Setting,PermissionKey.HostSettings),
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
            foreach (var menuItem in GetMenuItems())
            {
                //转换特定地区语言的标题
                menuItem.Title = Local.Localize(menuItem.Title);

                if (menuItem.RequiredPermissionName == null ||
                    (permissions != null && permissions.ContainsKey(menuItem.RequiredPermissionName)))
                {
                    if (menuItem.Items != null)
                    {
                        foreach (var submenuItem in menuItem.Items)
                            submenuItem.Title = Local.Localize(submenuItem.Title);
                    }
                    authorizedMenuItems.Add(menuItem);
                }
            }
            return authorizedMenuItems;
        }
    }
}