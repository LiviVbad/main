using System; 
using System.Linq; 
using System.Threading.Tasks; 
using AppFrameworkDemo.ApiClient;
using AppFrameworkDemo.Configuration;
using AppFrameworkDemo.MultiTenancy;

namespace AppFramework.Services.Account
{
    public class UserConfigurationManager : IUserConfigurationManager
    {
        private readonly IApplicationContext applicationContext;
        private readonly IAccessTokenManager accessTokenManager;
        private readonly UserConfigurationService userConfigurationService;

        public UserConfigurationManager(IApplicationContext applicationContext,
            IAccessTokenManager accessTokenManager,
            UserConfigurationService userConfigurationService)
        {
            this.applicationContext = applicationContext;
            this.accessTokenManager = accessTokenManager;
            this.userConfigurationService = userConfigurationService;
        }

        public async Task GetConfigurationAsync(
            Func<string, Task> OnAccessTokenRefresh = null,
            Func<Task> OnSessionTimeOut = null,
            Func<Task> successCallback = null)
        {
            if (OnAccessTokenRefresh != null)
                userConfigurationService.OnAccessTokenRefresh = OnAccessTokenRefresh;

            if (OnSessionTimeOut != null)
                userConfigurationService.OnSessionTimeOut = OnSessionTimeOut;

            await WebRuner.Execute(
                async () => await userConfigurationService.GetAsync(accessTokenManager.IsUserLoggedIn),
                async result =>
                {
                    if (!result.MultiTenancy.IsEnabled)
                        applicationContext.SetAsTenant(TenantConsts.DefaultTenantName, TenantConsts.DefaultTenantId);

                    applicationContext.Configuration = result;
                    applicationContext.CurrentLanguage = result.Localization.CurrentLanguage;

                    WarnIfUserHasNoPermission();
                    if (successCallback != null)
                        await successCallback();
                }, ex =>
                { 
                    return Task.CompletedTask;
                });
        }

        private void WarnIfUserHasNoPermission()
        {
            if (!accessTokenManager.IsUserLoggedIn)
            {
                return;
            }

            var hasAnyPermission = applicationContext.Configuration.Auth.GrantedPermissions != null &&
                                   applicationContext.Configuration.Auth.GrantedPermissions.Any();

            if (!hasAnyPermission)
            {
                //UserDialogHelper.Warn("NoPermission");
            }
        }
    }
}
