using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFramework.Startup
{
    [DependsOn(typeof(AppFrameworkCoreModule))]
    public class AppFrameworkGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}