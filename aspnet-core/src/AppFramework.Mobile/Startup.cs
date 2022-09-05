using Prism.Ioc;

#region ApplicationServices

using AppFramework.Shared.ViewModels;
using AppFramework.Shared.Views;
using AppFramework.Shared.Services.Account;
using AppFramework.Shared.Services.Storage;
using AppFramework.Shared.Services.Layer;
using AppFramework.Shared.Services;
using AppFramework.Shared.Validations; 
using AppFramework.Shared.Services.Navigation; 

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
            services.RegisterCommonServices();
             
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
            services.RegisterSingleton<IRegionNavigateService, RegionNavigateService>();
            services.RegisterSingleton<IApplayerService, ApplayerService>();
            services.RegisterSingleton<IApplicationService, ApplicationService>();
            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();

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
    }
}