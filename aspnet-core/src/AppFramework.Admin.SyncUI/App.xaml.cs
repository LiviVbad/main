using AppFramework.Shared;
using AppFramework.Shared.Services.App;
using AppFramework.Shared.Services.Mapper;
using Hardcodet.Wpf.TaskbarNotification;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Syncfusion.Windows.Tools.Controls;
using System.Windows;

namespace AppFramework.Admin.SyncUI
{ 
    public partial class App : PrismApplication, IAppTaskBar
    { 
        private TaskbarIcon taskBar;

        protected override Window? CreateShell() => null;

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.AddSharedServices();
            container.ConfigureService();

            container.RegisterSingleton<IAppMapper, AppMapper>();
            container.RegisterSingleton<ILocaleCulture, LocaleCulture>();
            container.RegisterSingleton<IUpdateService, UpdateService>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping<TabControlExt, SfTabControlRegionAdapter>();
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override async void OnInitialized()
        {
            Initialization();

            var appVersionService = ContainerLocator.Container.Resolve<IUpdateService>();
            await appVersionService.CheckVersion();

            var appStart = ContainerLocator.Container.Resolve<IAppStartService>();
            MainWindow =  appStart.CreateShell(this);

            base.OnInitialized();
        }

        public void Initialization()
        {
            taskBar = (TaskbarIcon)FindResource("taskBar");

            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5NjkxQDMyMzAyZTMxMmUzMG4yeGhZNm01STJSdnVKQVJiUHpzM3ZMUEc5K1hZTXd3TVFTbGZ1UERrQlU9");
        }

        public void Dispose() => taskBar?.Dispose();
    }
}