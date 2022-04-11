using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using AppFrameworkDemo.Configuration;

namespace AppFrameworkDemo.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(AppFrameworkDemoTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
