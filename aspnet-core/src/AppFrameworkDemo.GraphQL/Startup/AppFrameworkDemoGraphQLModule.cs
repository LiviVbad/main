using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFramework.Startup
{
    [DependsOn(typeof(AppFrameworkCoreModule))]
    public class AppFrameworkDemoGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkDemoGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}