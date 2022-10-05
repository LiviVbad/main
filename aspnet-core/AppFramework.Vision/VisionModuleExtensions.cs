using AppFramework.Vision.Services;
using AppFramework.Vision.ViewModels;
using AppFramework.Vision.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace AppFramework.Vision
{
    /*
     * Halcon机器视觉模块
     * 
     * 包含Halcon中内置的常用视觉匹配算法实现
     * 
     */


    public static class VisionModuleExtensions
    {
        public static void AddVisionServices(this IContainerRegistry services)
        {
            services.AddServices();
            services.AddViews();
        }
    }
}
