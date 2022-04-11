using Microsoft.Extensions.Configuration;

namespace AppFrameworkDemo.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
