using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFrameworkDemo.Startup
{
    [DependsOn(typeof(AppFrameworkDemoCoreModule))]
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