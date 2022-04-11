using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AppFrameworkDemo.Authorization;

namespace AppFrameworkDemo
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(AppFrameworkDemoSharedModule),
        typeof(AppFrameworkDemoCoreModule)
        )]
    public class AppFrameworkDemoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkDemoApplicationModule).GetAssembly());
        }
    }
}