using AppFramework.Extensions;
using AppFramework.Services;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;
using Prism.Modularity;
using AppFramework.Shared;
using AppFramework.Admin;
using AppFramework.Shared.Core;
using AppFramework.Shared.Services.App;
using System;

namespace AppFramework
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => null;

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILocaleCulture, LocaleCulture>();
            containerRegistry.RegisterSingleton<IThemeService, ThemeService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<AdminModule>();
            moduleCatalog.AddModule<SharedModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override IContainerExtension CreateContainerExtension()
        { 
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAutoMapper(config =>
            {
                config.AddProfile<AdminMapper>();
                config.AddProfile<SharedMapper>();
            });
            return new DryIocContainerExtension(new Container(CreateContainerRules())
                .WithDependencyInjectionAdapter(serviceCollection));
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.ConfigurationAdapters(Container);
        }

        protected override async void OnInitialized()
        {
            AppSettings.OnInitialized();

            var appStart = ContainerLocator.Container.Resolve<IAppStartService>();
            MainWindow = await appStart.CreateShell(this);

            base.OnInitialized();
        }
    }
}