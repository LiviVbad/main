using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using AppFramework.Configuration;

namespace AppFramework.Test.Base
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
