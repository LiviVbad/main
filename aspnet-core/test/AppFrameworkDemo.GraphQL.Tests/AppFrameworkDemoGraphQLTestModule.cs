using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using AppFramework.Configure;
using AppFramework.Startup;
using AppFramework.Test.Base;

namespace AppFramework.GraphQL.Tests
{
    [DependsOn(
        typeof(AppFrameworkDemoGraphQLModule),
        typeof(AppFrameworkDemoTestBaseModule))]
    public class AppFrameworkDemoGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkDemoGraphQLTestModule).GetAssembly());
        }
    }
}