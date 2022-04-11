using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFrameworkDemo
{
    [DependsOn(typeof(AppFrameworkDemoCoreSharedModule))]
    public class AppFrameworkDemoSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkDemoSharedModule).GetAssembly());
        }
    }
}