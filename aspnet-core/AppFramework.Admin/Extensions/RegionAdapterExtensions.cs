using Prism.Ioc;
using Prism.Regions; 
using Syncfusion.Windows.Tools.Controls; 

namespace AppFramework.Extensions
{
    /// <summary>
    /// 适配器扩展服务
    /// </summary>
    public static class RegionAdapterExtensions
    { 
        public static void ConfigurationAdapters(
            this RegionAdapterMappings regionAdapterMappings,
            IContainerProvider container)
        {
            regionAdapterMappings.RegisterMapping(typeof(TabControlExt),
                container.Resolve<SfTabControlRegionAdapter>());
        }
    }
}