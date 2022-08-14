using AppFramework.Shared.Services.App;
using System;
using System.Windows;
using Prism.Ioc;
using AppFramework.Services.Update;
using AppFramework.Shared.Services.Account;
using System.Threading.Tasks;
using AppFramework.Shared;
using Prism.Regions;
using Syncfusion.Windows.Shared;
using AppFramework.Services;
using Prism.Services.Dialogs;
using AppFramework.Shared.Services.Storage;
using AppFramework.Configuration;

namespace AppFramework.Admin
{
    public class AppStartService : IAppStartService
    {
        private System.Windows.Application app;

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void Logout()
        {
            app.MainWindow.Hide();

            if (SplashScreenInitialized())
            {
                app.MainWindow.Show();
                (app.MainWindow.DataContext as INavigationAware)?.OnNavigatedTo(null);
            }
            else
                Environment.Exit(0);
        }

        public async Task<Window> CreateShell(System.Windows.Application app)
        {
            this.app = app;
            var container = ContainerLocator.Container;

            var userConfigurationService = container.Resolve<UserConfigurationService>();
            userConfigurationService.OnAccessTokenRefresh = OnAccessTokenRefresh;
            userConfigurationService.OnSessionTimeOut = OnSessionTimeout;

            var appVersionService = container.Resolve<IUpdateService>();
            await appVersionService.CheckVersion();

            if (SplashScreenInitialized())
            {
                var shell = container.Resolve<object>(AppViews.Main);
                if (shell is ChromelessWindow view)
                {
                    var regionManager = container.Resolve<IRegionManager>();
                    RegionManager.SetRegionManager(view, regionManager);
                    RegionManager.UpdateRegions();

                    if (view.DataContext is INavigationAware navigationAware)
                    {
                        navigationAware.OnNavigatedTo(null);
                        return (Window)shell;
                    }
                }
            }

            return null;
        }

        private static bool SplashScreenInitialized()
        {
            var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
            var result = dialogService.ShowWindow(AppViews.SplashScreen).Result;
            if (result == ButtonResult.No)
            {
                if (!Authorization()) ExitApplication();
            }
            else if (result == ButtonResult.None) ExitApplication();

            return true;
        }

        private static bool Authorization()
        {
            var validationResult = Validation();
            if (validationResult == ButtonResult.Retry)
                return Authorization();

            return validationResult == ButtonResult.OK;

            static ButtonResult Validation()
            {
                var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
                return dialogService.ShowWindow(AppViews.Login).Result;
            }
        }

        public static void ExitApplication() => Environment.Exit(0);

        public static async Task OnSessionTimeout()
        {
            await ContainerLocator.Container.Resolve<IAccountService>().LogoutAsync();
        }

        public static async Task OnAccessTokenRefresh(string newAccessToken)
        {
            await ContainerLocator.Container.Resolve<IAccountStorageService>().StoreAccessTokenAsync(newAccessToken);
        }
    }
}
