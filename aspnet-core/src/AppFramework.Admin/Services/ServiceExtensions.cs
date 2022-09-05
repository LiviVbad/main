using Prism.Ioc;
using AppFramework.Shared.Services.App;
using AppFramework.Shared.Services.Storage;
using AppFramework.Services;
using AppFramework.Services.Account;
using AppFramework.Services.Notification;
using AppFramework.ApiClient; 
using AppFramework.Shared;

namespace AppFramework.Admin
{ 
    public static class ServiceExtensions
    { 
        public static void AddServices(this IContainerRegistry services)
        {
            services.RegisterSingleton<IAppStartService, AppStartService>();
            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();
            services.RegisterScoped<IPermissionTreesService, PermissionTreesService>();
            services.Register<IPermissionPorxyService, PermissionPorxyService>();
            services.RegisterScoped<IFeaturesService, FeaturesService>();

            services.RegisterSingleton<IApplicationService, ApplicationService>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
            services.RegisterSingleton<NavigationService>();
            services.RegisterSingleton<NotificationService>();
        }
    }
}
