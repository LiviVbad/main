using Prism.Ioc;

#region ApplicationServices

using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Users;
using AppFramework.MultiTenancy;
using AppFramework.Editions;
using AppFramework.Auditing;
using AppFramework.DynamicEntityProperties;
using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Configuration;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Accounts;
using AppFramework.Localization;
using AppFramework.Sessions;
using AppFramework.MultiTenancy.HostDashboard;
using AppFramework.Caching;
using AppFramework.Tenants.Dashboard;
using AppFramework.Organizations;
using AppFramework.Application;
using AppFramework.Application.Client;
using AppFramework.Application.MultiTenancy.HostDashboard;
using AppFramework.Application.Organizations;
using AppFramework.Application.MultiTenancy;
using AppFramework.Shared.Core; 
using Abp.Configuration.Startup;
using AppFramework.Authorization.Permissions; 
using AppFramework.Shared.Services;
using AppFramework.Configuration.Host; 
using AppFramework.Notifications;
using AppFramework.Authorization.Users.Delegation;
using AppFramework.Version;
using AppFramework.Common;

#endregion ApplicationServices

namespace AppFramework.Shared
{
    public static class AppSharedServicesExtensions
    {
        /// <summary>
        /// 注册公共服务
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterCommonServices(this IContainerRegistry services)
        {
            /*    
            *  包含如下: 角色服务、用户服务、租户服务、版本服务、
            *           审计日志服务、语言服务、组织机构服务
            *           动态属性服务、缓存服务、租户面板统计服务
            *           动态实体服务、账号服务、个人资料服务、
            *           统计面板服务 ...
            */

            services.RegisterScoped<IRoleAppService, RoleAppService>();
            services.RegisterScoped<IUserAppService, ProxyUserAppService>();
            services.RegisterScoped<IUserLoginAppService, UserLoginAppService>();
            services.RegisterScoped<IUserDelegationAppService, UserDelegationAppService>();
            services.RegisterScoped<ITenantAppService, ProxyTenantAppService>();
            services.RegisterScoped<IEditionAppService, EditionAppService>();
            services.RegisterScoped<IAuditLogAppService, AuditLogAppService>();
            services.RegisterScoped<ILanguageAppService, LanguageAppService>();
            services.RegisterScoped<IPermissionTreesService, PermissionTreesService>();
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
            services.Register<IPermissionPorxyService, PermissionPorxyService>();
            services.RegisterScoped<IFeaturesService, FeaturesService>();
            services.RegisterScoped<IUserLinkAppService, UserLinkAppService>();
            services.RegisterScoped<IAbpVersionsAppService, AbpVersionsAppService>();

            services.RegisterSingleton<IMessenger, Messenger>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();
            services.RegisterSingleton<IMultiTenancyConfig, MultiTenancyConfig>();
            services.RegisterSingleton<IApplicationContext, ApplicationContext>();
            services.RegisterSingleton<AbpApiClient>();
            services.RegisterSingleton<AbpAuthenticateModel>();
            services.RegisterScoped<UserConfigurationService>();
            services.RegisterScoped<ProxyProfileControllerService>();
            services.RegisterScoped<ProxyTokenAuthControllerService>();
        }
    }
}
