﻿using Abp.Configuration.Startup;
using AppFramework.ApiClient.Models;
using AppFramework.ApiClient;
using AppFramework.Application.Client;
using AppFramework.Application.MultiTenancy.HostDashboard;
using AppFramework.Application.MultiTenancy;
using AppFramework.Application.Organizations;
using AppFramework.Application;
using AppFramework.Auditing;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Permissions;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Users.Delegation;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users;
using AppFramework.Caching;
using AppFramework.Common;
using AppFramework.Configuration.Host;
using AppFramework.Configuration;
using AppFramework.DynamicEntityProperties;
using AppFramework.Editions;
using AppFramework.Localization;
using AppFramework.MultiTenancy.HostDashboard;
using AppFramework.MultiTenancy;
using AppFramework.Notifications;
using AppFramework.Organizations;
using AppFramework.Sessions;
using AppFramework.Tenants.Dashboard;
using AppFramework.Version;
using Prism.Ioc;
using Prism.Modularity;
using AppFramework.Shared.Services;
using AppFramework.Services;
using AppFramework.Services.Dialog; 
using AppFramework.Shared.Validations;

namespace AppFramework.Shared
{
    public class SharedModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        { }

        public void RegisterTypes(IContainerRegistry services)
        {
            services.RegisterSingleton<IGlobalValidator, GlobalValidator>();

            services.Register<IDataPagerService, DataPagerService>();
            services.RegisterSingleton<IHostDialogService, DialogHostService>();
          
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();
            services.RegisterSingleton<IMultiTenancyConfig, MultiTenancyConfig>();
            services.RegisterSingleton<IApplicationContext, ApplicationContext>();
            services.RegisterSingleton<AbpApiClient>();
            services.RegisterSingleton<AbpAuthenticateModel>();
            services.RegisterSingleton<UserConfigurationService>();
            services.RegisterScoped<ProxyProfileControllerService>();
            services.RegisterScoped<ProxyTokenAuthControllerService>();

            services.RegisterScoped<IRoleAppService, RoleAppService>();
            services.RegisterScoped<IUserAppService, ProxyUserAppService>();
            services.RegisterScoped<IUserLoginAppService, UserLoginAppService>();
            services.RegisterScoped<IUserDelegationAppService, UserDelegationAppService>();
            services.RegisterScoped<ITenantAppService, ProxyTenantAppService>();
            services.RegisterScoped<IEditionAppService, EditionAppService>();
            services.RegisterScoped<IAuditLogAppService, AuditLogAppService>();
            services.RegisterScoped<ILanguageAppService, LanguageAppService>();
         
            services.RegisterScoped<INotificationAppService, NotificationAppService>();
            services.RegisterScoped<IOrganizationUnitAppService, OrganizationUnitAppService>();
            services.RegisterScoped<IDynamicPropertyAppService, DynamicPropertyAppService>();
            services.RegisterScoped<ICachingAppService, CachingAppService>();
            services.RegisterScoped<ITenantDashboardAppService, TenantDashboardAppService>();
            services.RegisterScoped<IDynamicEntityPropertyAppService, DynamicEntityPropertyAppService>();
            services.RegisterScoped<IDynamicEntityPropertyDefinitionAppService, DynamicEntityPropertyDefinitionAppService>();
            services.RegisterScoped<IDynamicPropertyValueAppService, DynamicPropertyValueAppService>();
            services.RegisterScoped<ICommonLookupAppService, ProxyCommonLookupAppService>();
            services.RegisterScoped<IAccountAppService, ProxyAccountAppService>();
            services.RegisterScoped<IProfileAppService, ProxyProfileAppService>();
            services.RegisterScoped<ISessionAppService, ProxySessionAppService>();
            services.RegisterScoped<IHostDashboardAppService, HostDashboardAppService>();
            services.RegisterScoped<IHostSettingsAppService, HostSettingsAppService>();
            services.RegisterScoped<IPermissionAppService, PermissionAppService>();
       
            services.RegisterScoped<IUserLinkAppService, UserLinkAppService>();
            services.RegisterScoped<IAbpVersionsAppService, AbpVersionsAppService>();
        }
    }
}
