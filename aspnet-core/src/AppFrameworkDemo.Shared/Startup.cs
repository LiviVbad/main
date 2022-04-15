using Prism.Ioc;
using Abp.Configuration.Startup;

#region ApplicationServices

using AppFramework.Shared.ViewModels;
using AppFramework.Shared.Views;
using AppFramework.Shared.Services.Account; 
using AppFramework.Shared.Services.Storage;
using AppFramework.Shared.Services.Layer;
using AppFramework.Shared.Services;
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
using AppFramework.Common;
using AppFramework.Organizations; 
using AppFramework.Application;
using AppFramework.Application.Client;
using AppFramework.Application.MultiTenancy.HostDashboard;
using AppFramework.Application.Organizations;
using AppFramework.Application.MultiTenancy;
using AppFramework.Common.Core.Validations;
using AppFramework.Common.Core;
using AppFramework.Common.Services;
using AppFramework.Common.Services.Navigation;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Layer;
using AppFramework.Common.Services.Storage;
using AppFramework.Common.Services.Permission;

#endregion ApplicationServices

namespace AppFramework.Shared
{
    internal static class Startup
    {
        /// <summary>
        /// 配置应用程序模块服务
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSerivces(this IContainerRegistry services)
        {
            //注册应用程序依赖服务
            services.ConfigureAppServices();

            //注册应用程序验证器
            services.RegisterValidator();

            /*
             *  注册应用程序模块 (Prism区域导航 ContenView)
             *  
             *  包含如下:登录页、系统首页、 统计面板、用户视图、
             *          角色视图、语言视图 租户视图、版本视图、
             *          审计日志视图、动态属性视图、
             *          组织机构视图、设置视图。
             */
            services.RegisterForRegionNavigation<LoginView, LoginViewModel>(AppViewManager.Login);
            services.RegisterForRegionNavigation<MainView, MainViewModel>(AppViewManager.Main);
            services.RegisterForRegionNavigation<DashboardView, DashboardViewModel>(AppViewManager.Dashboard);
            services.RegisterForRegionNavigation<UserView, UserViewModel>(AppViewManager.User);
            services.RegisterForRegionNavigation<RoleView, RoleViewModel>(AppViewManager.Role);
            services.RegisterForRegionNavigation<LanguageView, LanguageViewModel>(AppViewManager.Language);
            services.RegisterForRegionNavigation<TenantView, TenantViewModel>(AppViewManager.Tenant);
            services.RegisterForRegionNavigation<EditionView, EditionViewModel>(AppViewManager.Edition);
            services.RegisterForRegionNavigation<AuditLogView, AuditLogViewModel>(AppViewManager.AuditLog);
            services.RegisterForRegionNavigation<DynamicPropertyView, DynamicPropertyViewModel>(AppViewManager.DynamicProperty);
            services.RegisterForRegionNavigation<OrganizationView, OrganizationViewModel>(AppViewManager.Organization);
            services.RegisterForRegionNavigation<SettingsView, SettingsViewModel>(AppViewManager.Setting);

            /*
             *  注册应用程序页面 (导航页 ContentPage)
             * 
             *  包含如下: 初始化页、 用户详情页、
             *           角色详情页、租户详情页、
             *           版本详情页、语言详情页、
             *           新增角色页、新增用户页、
             *           组织机构详情页、动态熟悉详情页、
             *           个人设置页、忘记密码页、
             *           修改密码页、邮件激活页
             *           头像详情页、双重验证页
             */

            services.RegisterDialog<MyProfileView, MyProfileViewModel>();
            services.RegisterDialog<MessageBoxView, MessageBoxViewModel>();
            services.RegisterDialog<ForgotPasswordView, ForgotPasswordViewModel>();
            services.RegisterDialog<ChangePasswordView, ChangePasswordViewModel>();
            services.RegisterDialog<EmailActivationView, EmailActivationViewModel>();
            services.RegisterDialog<SendTwoFactorCodeView, SendTwoFactorCodeViewModel>();
            services.RegisterForNavigation<InitialScreenView, InitialScreenViewModel>();
            services.RegisterForNavigation<UserDetailsView, UserDetailsViewModel>();
            services.RegisterForNavigation<RoleDetailsView, RoleDetailsViewModel>();
            services.RegisterForNavigation<TenantDetailsView, TenantDetailsViewModel>();
            services.RegisterForNavigation<EditionDetailsView, EditionDetailsViewModel>();
            services.RegisterForNavigation<LanguageDetailsView, LanguageDetailsViewModel>();
            services.RegisterForNavigation<AddRolesView, AddRolesViewModel>();
            services.RegisterForNavigation<AddUsersView, AddUsersViewModel>();
            services.RegisterForNavigation<OrganizationDetailsView, OrganizationDetailsViewModel>();
            services.RegisterForNavigation<DynamicPropertyDetailsView, DynamicPropertyDetailsViewModel>(); 
            services.RegisterForNavigation<ProfilePictureView, ProfilePictureViewModel>();
        }

        public static void ConfigureAppServices(this IContainerRegistry services)
        {
            /*  注册模块Web服务
             * 
             *  包含如下: 角色服务、用户服务、租户服务、版本服务、
             *           审计日志服务、语言服务、组织机构服务
             *           动态属性服务、缓存服务、租户面板统计服务
             *           动态实体服务、账号服务、个人资料服务、
             *           统计面板服务 ...
             */
            services.RegisterScoped<IRoleAppService, RoleAppService>();
            services.RegisterScoped<IUserAppService, ProxyUserAppService>();
            services.RegisterScoped<ITenantAppService, ProxyTenantAppService>();
            services.RegisterScoped<IEditionAppService, EditionAppService>();
            services.RegisterScoped<IAuditLogAppService, AuditLogAppService>();
            services.RegisterScoped<ILanguageAppService, LanguageAppService>();
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

            /*  注册应用程序配置及授权服务
             * 
             *  包含如下: 应用程序弹窗服务、发布订阅消息服务、区域导航服务、
             *           应用程序信息服务、用户授权服务、权限服务、
             *           本地存储服务、账号密钥服务
             *           Web请求服务、授权信息实体、多租户配置
             *           应用程序上下文、用户配置服务、头像上传服务
             *           双重验证服务
             */
            services.RegisterSingleton<IMessenger, Messenger>();
            services.RegisterSingleton<IRegionNavigateService, RegionNavigateService>();
            services.RegisterSingleton<IApplayerService, ApplayerService>();
            services.RegisterSingleton<IApplicationService, ApplicationService>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();
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