using Prism.Ioc;
using Prism.Regions;
using Syncfusion.UI.Xaml.NavigationDrawer;

namespace AppFramework.Extensions
{
    public static class RegionAdapterExtensions
    {
        public static void ConfigurationAdapters(
            this RegionAdapterMappings regionAdapterMappings,
            IContainerProvider container)
        {
            regionAdapterMappings.RegisterMapping(typeof(SfNavigationDrawer),
                container.Resolve<SfNavigationDrawerRegionAdapter>());
        }
    }
}