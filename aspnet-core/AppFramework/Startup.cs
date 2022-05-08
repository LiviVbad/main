using AppFramework.ApiClient;
using AppFramework.Common;
using AppFramework.Common.Core.Validations;
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

namespace AppFramework
{
    public static class Startup
    {
        public static void ConfigurationServices(this IContainerRegistry services)
        {
            //注册应用程序依赖服务
            services.RegisterCommonServices();

            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();

            //注册应用程序验证器
            services.RegisterValidator();

            services.RegisterSingleton<ILocaleCulture, LocaleCulture>();
            services.RegisterSingleton<IThemeService, ThemeService>();
            services.RegisterSingleton<IResourceService, ResourceService>();
            services.RegisterSingleton<IHostDialogService, DialogHostService>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
             
            services.RegisterDialog<LoginView, LoginViewModel>(AppViewManager.Login);

            services.Add<MainView, MainViewModel>(AppViewManager.Main);
            services.Add<HostMessageBoxView, HostMessageViewModel>(AppViewManager.HostMessageBox);
            services.Add<MessageBoxView, MessageViewModel>(AppViewManager.MessageBox);
            services.Add<UserView, UserViewModel>(AppViewManager.User);
            services.Add<UserDetailsView, UserDetailsViewModel>(AppViewManager.UserDetails);
            services.Add<RoleView, RoleViewModel>(AppViewManager.Role);
            services.Add<RoleDetailsView, RoleDetailsViewModel>(AppViewManager.RoleDetails);

            services.Add<EditionView, EditionViewModel>(AppViewManager.Edition);
            services.Add<EditionDetailsView, EditionDetailsViewModel>(AppViewManager.EditionDetails);
            services.Add<DynamicPropertyView, DynamicPropertyViewModel>(AppViewManager.DynamicProperty);
            services.Add<DynamicPropertyDetailsView, DynamicPropertyDetailsViewModel>(AppViewManager.DynamicPropertyDetails);
            services.Add<TenantView, TenantViewModel>(AppViewManager.Tenant);
            services.Add<TenantDetailsView, TenantDetailsViewModel>(AppViewManager.TenantDetails);
            services.Add<AddRolesView, AddRolesViewModel>(AppViewManager.AddRoles);
            services.Add<AddUsersView, AddUsersViewModel>(AppViewManager.AddUsers);
            services.Add<AuditLogsView, AuditLogsViewModel>(AppViewManager.AuditLog);
            services.Add<LanguageView, LanguageViewModel>(AppViewManager.Language);
            services.Add<VisualView, VisualViewModel>(AppViewManager.Visual);
            services.Add<DashboardView, DashboardViewModel>(AppViewManager.Dashboard);
            services.Add<OrganizationsView, OrganizationsViewModel>(AppViewManager.Organization);
            services.Add<OrganizationsAddView, OrganizationsAddViewModel>(AppViewManager.OrganizationAdd);
        }

        private static void Add<TView, TViewModel>(this IContainerRegistry containerRegistry, string name = null)
        {
            containerRegistry.RegisterForNavigation<TView, TViewModel>(name);
        }
    }
}