using AppFramework.ApiClient;
using AppFramework.Common;
using AppFramework.Common.Validations;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Navigation;
using AppFramework.Common.Services.Storage;
using AppFramework.Services;
using AppFramework.Services.Account;
using AppFramework.Services.Dialog;
using AppFramework.ViewModels;
using AppFramework.Views;
using AppFramework.Localization;
using Prism.Ioc;
using AppFramework.Common.Services;
using AppFramework.ViewModels.Shared;
using AppFramework.Services.Notification;

namespace AppFramework
{
    public static class Startup
    {
        public static void ConfigurationServices(this IContainerRegistry services)
        {
            //注册应用程序依赖服务
            services.RegisterCommonServices();
            //注册应用程序验证器
            services.RegisterValidator();

            /*
             *  《应用程序授权相关服务》
             *  账户服务、授权缓存服务、权限验证服务、Token服务
             */

            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();

            /*
             * 《应用程序内部功能服务》
             * 分页服务、本地化服务、主题服务、
             * 对话窗口服务、应用程序资源服务、导航菜单服务
             */
            services.Register<IDataPagerService, DataPagerService>();
            services.RegisterSingleton<ILocaleCulture, LocaleCulture>();
            services.RegisterSingleton<IThemeService, ThemeService>();
            services.RegisterSingleton<IHostDialogService, DialogHostService>();
            services.RegisterSingleton<IApplicationService, ApplicationService>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
            services.RegisterSingleton<NavigationService>();
            services.RegisterSingleton<NotificationService>();

            /*
             * 《注册应用程序模块》
             * 
             * 顺序如下: 
             * 登录页、首页、会话消息窗口、弹出消息窗口
             * 用户页、用户详情页、用户修改权限页
             * 角色页、角色详情页、选择权限页
             * 版本列表页、版本列表详情页
             * 动态属性页、动态属性详情页、动态实体页、动态属性编辑值页
             * 租户页、租户详情页、添加角色页、添加成员页 
             * 审计日志页、审计日志详情页
             * 语言页、语言文本详情页、语言详情页
             * 统计面板页、组织机构页、新增组织机构页、系统设置页
             * 演示UI组件页
             * 
             */
            services.RegisterDialog<SplashScreenView, SplashScreenViewModel>(AppViewManager.SplashScreen);
            services.RegisterDialog<LoginView, LoginViewModel>(AppViewManager.Login);

            services.Add<FirstChangedPwdView, FirstChangedPwdViewModel>(AppViewManager.FirstChangedPwd);
            services.Add<SelectTenantView, SelectTenantViewModel>(AppViewManager.SelectTenant);
            services.Add<MainView, MainViewModel>(AppViewManager.Main);
            services.Add<MainTabsView, MainTabsViewModel>(AppViewManager.Main);
            services.Add<HostMessageBoxView, HostMessageViewModel>(AppViewManager.HostMessageBox);
            services.Add<MessageBoxView, MessageViewModel>(AppViewManager.MessageBox);
            services.Add<UserView, UserViewModel>(AppViewManager.User);
            services.Add<UserDetailsView, UserDetailsViewModel>(AppViewManager.UserDetails);
            services.Add<UserChangePermissionView, UserChangePermissionViewModel>(AppViewManager.UserChangePermission);
            services.Add<RoleView, RoleViewModel>(AppViewManager.Role);
            services.Add<RoleDetailsView, RoleDetailsViewModel>(AppViewManager.RoleDetails);
            services.Add<SelectedPermissionView, SelectedPermissionViewModel>(AppViewManager.SelectedPermission);
            services.Add<EditionView, EditionViewModel>(AppViewManager.Edition);
            services.Add<EditionDetailsView, EditionDetailsViewModel>(AppViewManager.EditionDetails);
            services.Add<DynamicPropertyView, DynamicPropertyViewModel>(AppViewManager.DynamicProperty);
            services.Add<DynamicPropertyDetailsView, DynamicPropertyDetailsViewModel>(AppViewManager.DynamicPropertyDetails);
            services.Add<DynamicAddEntityView, DynamicAddEntityViewModel>(AppViewManager.DynamicAddEntity);
            services.Add<DynamicEntityDetailsView, DynamicEntityDetailsViewModel>(AppViewManager.DynamicEntityDetails);
            services.Add<DynamicEditValuesView, DynamicEditValuesViewModel>(AppViewManager.DynamicEditValues);
            services.Add<TenantView, TenantViewModel>(AppViewManager.Tenant);
            services.Add<TenantDetailsView, TenantDetailsViewModel>(AppViewManager.TenantDetails);
            services.Add<TenantChangeFeaturesView, TenantChangeFeaturesViewModel>(AppViewManager.TenantChangeFeatures);
            services.Add<AddRolesView, AddRolesViewModel>(AppViewManager.AddRoles);
            services.Add<AddUsersView, AddUsersViewModel>(AppViewManager.AddUsers);
            services.Add<AuditLogsView, AuditLogsViewModel>(AppViewManager.AuditLog);
            services.Add<AuditLogsDetailsView, AuditLogsDetailsViewModel>(AppViewManager.AuditLogDetails);
            services.Add<LanguageView, LanguageViewModel>(AppViewManager.Language);
            services.Add<LanguageTextView, LanguageTextViewModel>(AppViewManager.LanguageText);
            services.Add<LanguageTextDetailsView, LanguageTextDetailsViewModel>(AppViewManager.LanguageTextDetails);
            services.Add<LanguageDetailsView, LanguageDetailsViewModel>(AppViewManager.LanguageDetails);
            services.Add<DashboardView, DashboardViewModel>(AppViewManager.Dashboard);
            services.Add<OrganizationsView, OrganizationsViewModel>(AppViewManager.Organization);
            services.Add<OrganizationsAddView, OrganizationsAddViewModel>(AppViewManager.OrganizationAdd);
            services.Add<SettingsView, SettingsViewModel>(AppViewManager.Setting);
            services.Add<DemoUiView, DemoUiViewModel>(AppViewManager.Demo); //演示组件页
            services.Add<VisualView, VisualViewModel>(AppViewManager.Visual);
            services.Add<VersionManagerView, VersionManagerViewModel>(AppViewManager.Update);

            services.Add<NotificationView, NotificationViewModel>(AppViewManager.Notification);
            services.Add<MyProfileView, MyProfileViewModel>(AppViewManager.MyProfile);
            services.Add<LoginAttemptsView, LoginAttemptsViewModel>(AppViewManager.LoginAttempts);
            services.Add<ManageLinkedAccountsView, ManageLinkedAccountsViewModel>(AppViewManager.ManageLinkedAccounts);
            services.Add<ManageUserDelegationsView, ManageUserDelegationsViewModel>(AppViewManager.ManageUserDelegations);
            services.Add<SelectDateRangeView, SelectDateRangeViewModel>(AppViewManager.SelectDate);
            services.Add<ManageNewUserView, ManageNewUserViewModel>(AppViewManager.ManageNewUser);
            services.Add<CreateLinkedAccountView, CreateLinkedAccountViewModel>(AppViewManager.CreateLinkedAccount);
            services.Add<MySettingsView, MySettingsViewModel>(AppViewManager.MySetting);
            services.Add<ChangeAvatarView, ChangeAvatarViewModel>(AppViewManager.ChangeAvatar);
            services.Add<ChangePasswordView, ChangePasswordViewModel>(AppViewManager.ChangePassword);
            services.Add<EmailActivationView, EmailActivationViewModel>(AppViewManager.EmailActivation);
            services.Add<ForgotPasswordView, ForgotPasswordViewModel>(AppViewManager.ForgotPassword);
            services.Add<SendTwoFactorCodeView, SendTwoFactorCodeViewModel>(AppViewManager.SendTwoFactorCode);
        }

        private static void Add<TView, TViewModel>(this IContainerRegistry containerRegistry, string name = null)
        {
            containerRegistry.RegisterForNavigation<TView, TViewModel>(name);
        }
    }
}