using System;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Common;
using AppFramework.ApiClient;
using AppFramework.Configuration;
using AppFramework.MultiTenancy;
using CodeShare.Shared.Localization.Resources;
using Prism.Ioc;
using System.Globalization;
using AppFramework.Localization;

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

            await WebRequest.Execute(
                async () => await userConfigurationService.GetAsync(accessTokenManager.IsUserLoggedIn),
                async result =>
                {
                    applicationContext.Configuration = result;
                    SetCurrentCulture();

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

        /// <summary>
        /// 设置应用的区域化配置
        /// </summary>
        private void SetCurrentCulture()
        {
            var locale = ContainerLocator.Container.Resolve<ILocale>();
            var userCulture = GetUserCulture(locale);

            locale.SetLocale(userCulture);
            LocalTranslation.Culture = userCulture;
        }

        /// <summary>
        /// 获取用户的区域化信息
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        private CultureInfo GetUserCulture(ILocale locale)
        {
            if (applicationContext.Configuration.Localization.CurrentCulture.Name == null)
                return locale.GetCurrentCultureInfo();

            try
            {
                return new CultureInfo(applicationContext.Configuration.Localization.CurrentCulture.Name);
            }
            catch (CultureNotFoundException)
            {
                return locale.GetCurrentCultureInfo();
            }
        }
    }
}
