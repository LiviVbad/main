using AppFramework.ApiClient; 
using AppFramework.Common;
using AppFramework.Common.Core.Validations; 
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Navigation;
using AppFramework.Common.Services.Permission;
using AppFramework.Common.Services.Storage;
using AppFramework.Services;
using AppFramework.Services.Account;
using AppFramework.ViewModels;
using AppFramework.Views;
using AppFramework.WindowHost;
using Prism.Ioc; 

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

            services.RegisterSingleton<IThemeService, ThemeService>();
            services.RegisterSingleton<IResourceService, ResourceService>();

            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();
            services.RegisterSingleton<IPermissionService, PermissionService>(); 
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();
            services.RegisterSingleton<IUserConfigurationManager, UserConfigurationManager>();

            services.Register<IDialogHostService, DialogHostService>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();

            services.RegisterDialog<LoginView, LoginViewModel>();
            services.RegisterForNavigation<IndexView, IndexViewModel>();
            services.RegisterForNavigation<UserView, UserViewModel>();
            services.RegisterForNavigation<MainView, MainViewModel>();
            services.RegisterForNavigation<UserDetailsView, UserDetailsViewModel>();
            services.RegisterForNavigation<UserChooseView, UserChooseViewModel>();
            services.RegisterForNavigation<RoleView, RoleViewModel>();
            services.RegisterForNavigation<RoleChooseView, RoleChooseViewModel>();
            services.RegisterForNavigation<AuditLogsView, AuditLogsViewModel>();
            services.RegisterForNavigation<LanguageView, LanguageViewModel>();
            services.RegisterForNavigation<VisualView, VisualViewModel>();
            services.RegisterForNavigation<DashboardView, DashboardViewModel>();
            services.RegisterForNavigation<OrganizationsView, OrganizationsViewModel>();
            services.RegisterForNavigation<OrganizationsAddView, OrganizationsAddViewModel>();
        }
    }
}
