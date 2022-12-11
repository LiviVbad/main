﻿using AppFramework.Admin.Services;
using AppFramework.Configuration;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.App;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;

namespace AppFramework.Admin.MaterialUI
{
    internal class MaterialUIStartService : IAppStartService
    {
        public void CreateShell()
        {
            var container = ContainerLocator.Container;

            var userConfigurationService = container.Resolve<UserConfigurationService>();
            userConfigurationService.OnAccessTokenRefresh = OnAccessTokenRefresh;
            userConfigurationService.OnSessionTimeOut = OnSessionTimeout;

            this.SplashScreenInitialized();

            var shell = container.Resolve<object>(AppViews.Main);
            if (shell is Window view)
            {
                var regionManager = container.Resolve<IRegionManager>();
                RegionManager.SetRegionManager(view, regionManager);
                RegionManager.UpdateRegions();

                if (view.DataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(null);
                    App.Current.MainWindow = view;
                }
            }
        }

        private void SplashScreenInitialized()
        {
            var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
            var result = dialogService.ShowWindow(AppViews.SplashScreen).Result;
            if (result == ButtonResult.Ignore)
            {
                if (!Authorization()) Exit();
            }
            else if (result == ButtonResult.No) Exit();
        }

        private bool Authorization()
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

        public static async Task OnSessionTimeout()
        {
            await ContainerLocator.Container.Resolve<IAccountService>().LogoutAsync();
        }

        public static async Task OnAccessTokenRefresh(string newAccessToken)
        {
            await ContainerLocator.Container.Resolve<IAccountStorageService>().StoreAccessTokenAsync(newAccessToken);
        }

        public void Exit()
        {
            if (System.Windows.Application.Current is IAppTaskBar appTaskBar)
                appTaskBar.Dispose();

            Environment.Exit(0);
        }

        public void Logout()
        {
            App.Current.MainWindow.Hide();
            SplashScreenInitialized();
            App.Current.MainWindow.Show();

            if (App.Current.MainWindow.DataContext is INavigationAware navigationAware)
                navigationAware.OnNavigatedTo(null);
        }

    }
}
