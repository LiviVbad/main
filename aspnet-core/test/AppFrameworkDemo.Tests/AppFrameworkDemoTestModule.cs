using Abp.Modules;
using AppFrameworkDemo.Test.Base;

namespace AppFrameworkDemo.Tests
{
    [DependsOn(typeof(AppFrameworkDemoTestBaseModule))]
    public class AppFrameworkDemoTestModule : AbpModule
    {
       
    }
}
