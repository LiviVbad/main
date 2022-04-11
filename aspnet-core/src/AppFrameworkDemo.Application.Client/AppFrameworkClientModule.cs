using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFrameworkDemo
{
    public class AppFrameworkClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkClientModule).GetAssembly());
        }
    }
}