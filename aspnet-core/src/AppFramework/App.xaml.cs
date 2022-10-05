using AppFramework.Services;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System.Windows; 
using AppFramework.Shared;
using AppFramework.Shared.Services.App;
using Hardcodet.Wpf.TaskbarNotification;
using Services.Mapper;
using AppFramework.Shared.Services.Mapper;
using AppFramework.Admin;
using AppFramework.Vision;

namespace AppFramework
{
    public partial class App : PrismApplication
    {
        private TaskbarIcon taskBar;
        protected override Window CreateShell() => null;

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        { 
            containerRegistry.AddSharedServices();
            containerRegistry.AddAdminsServices();
            containerRegistry.AddVisionServices();

            containerRegistry.RegisterSingleton<IAppMapper, AppMapper>();
            containerRegistry.RegisterSingleton<ILocaleCulture, LocaleCulture>();
            containerRegistry.RegisterSingleton<IThemeService, ThemeService>();
            containerRegistry.RegisterSingleton<IUpdateService, UpdateService>();
        }
         
        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.ConfigurationAdapters(Container);
        }

        protected override async void OnInitialized()
        {  
            taskBar = (TaskbarIcon)FindResource("taskBar");

            AppSettings.OnInitialized();

            var appVersionService = ContainerLocator.Container.Resolve<IUpdateService>();
            await appVersionService.CheckVersion();

            var appStart = ContainerLocator.Container.Resolve<IAppStartService>();
            MainWindow = await appStart.CreateShell(this);

            base.OnInitialized();
        }
    }
}