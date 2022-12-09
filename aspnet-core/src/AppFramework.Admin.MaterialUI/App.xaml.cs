using AppFramework.Admin.Services;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.App;
using AppFramework.Shared.Services.Mapper;
using Hardcodet.Wpf.TaskbarNotification;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace AppFramework.Admin.MaterialUI
{
    public partial class App : PrismApplication, IAppTaskBar
    {
        private TaskbarIcon taskBar;

        protected override Window? CreateShell() => null;

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.AddViews();
            container.AddAdminsServices();
            container.AddSharedServices();

            //container.RegisterSingleton<IThemeService, ThemeService>();
            container.RegisterSingleton<IHostDialogService, DialogHostService>();
            container.RegisterSingleton<IAppStartService, MaterialUIStartService>();

            container.RegisterSingleton<IAppMapper, AppMapper>();
            container.RegisterSingleton<ILocaleCulture, LocaleCulture>();
            //container.RegisterSingleton<IUpdateService, UpdateService>();

            container.RegisterSingleton<INavigationMenuService, NavigationSingleMenuService>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override async void OnInitialized()
        {
            Initialization();

            //var appVersionService = ContainerLocator.Container.Resolve<IUpdateService>();
            //await appVersionService.CheckVersion();

            var appStart = ContainerLocator.Container.Resolve<IAppStartService>();
            appStart.CreateShell();

            base.OnInitialized();
        }

        public void Initialization()
        { 
            taskBar = (TaskbarIcon)FindResource("taskBar");
        }

        public void Dispose() => taskBar?.Dispose();
    }
}