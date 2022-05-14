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
               new NavigationItem("\ue6c4","Dashboard", AppViewManager.Dashboard, Permkeys.Administration),
               new NavigationItem("\uec07","Administration","",Permkeys.Administration,new ObservableCollection<NavigationItem>()
               {
                      new NavigationItem("\ue64e","OrganizationUnits",AppViewManager.Organization,Permkeys.OrganizationUnits),
                      new NavigationItem("\ue787","Roles",AppViewManager.Role,Permkeys.Roles),
                      new NavigationItem("\ue658","Users",AppViewManager.User,Permkeys.Users),
                      new NavigationItem("\ue617","AuditLogs",AppViewManager.AuditLog,Permkeys.AuditLogs),
                      new NavigationItem("\ue634","DynamicProperties",AppViewManager.DynamicProperty,Permkeys.DynamicProperties),
                      new NavigationItem("\ue635","Tenants",AppViewManager.Tenant,Permkeys.Tenants),
                      new NavigationItem("\ue657","Editions",AppViewManager.Edition,Permkeys.Editions),
               }),
               new NavigationItem("\ue62e","Languages",AppViewManager.Language,Permkeys.Languages),
               new NavigationItem("\ue600","Settings",AppViewManager.Setting,Permkeys.HostSettings),
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