using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFrameworkDemo
{
    public class AppFrameworkDemoCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkDemoCoreSharedModule).GetAssembly());
        }
    }
}