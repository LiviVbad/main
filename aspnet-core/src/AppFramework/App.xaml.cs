using AppFramework.Common;
using AppFramework.Common.Core;
using AppFramework.Views;
using AppFramework.Common.Services.Account;
using AppFramework.Extensions;
using AppFramework.Services;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AppFramework
{
    public partial class App : PrismApplication
    {
        private static IAccountService accountService;

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.ConfigurationServices();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAutoMapper(config =>
            {
                config.AddProfile<AppMapper>();
                config.AddProfile<AppCommonMapper>();
            });
            return new DryIocContainerExtension(new Container(CreateContainerRules())
                .WithDependencyInjectionAdapter(serviceCollection));
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.ConfigurationAdapters(Container);
        }

        protected override void OnInitialized()
        {
            accountService = Container.Resolve<IAccountService>();

            if (!Authorization()) Environment.Exit(0);

            (App.Current.MainWindow.DataContext as INavigationAware)?.OnNavigatedTo(null);
            base.OnInitialized();
        }

        private bool Authorization()
        {
            var validationResult = Validation(Container);
            if (validationResult == ButtonResult.Retry)
                return Authorization();

            return validationResult == ButtonResult.OK;

            static ButtonResult Validation(IContainerProvider container)
            {
                var dialogService = container.Resolve<IHostDialogService>();
                return dialogService.ShowWindow(AppViewManager.Login).Result;
            }
        }

        public static async Task OnSessionTimeout()
        {
            await accountService.LogoutAsync();
        }

        public static async Task OnAccessTokenRefresh(string newAccessToken)
        {
            await Task.CompletedTask;
        }
    }
}