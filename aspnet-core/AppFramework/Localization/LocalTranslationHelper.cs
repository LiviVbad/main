using System.Reflection;
using System.Resources;
using Prism.Ioc;

namespace AppFramework.Localization
{
    public static class LocalTranslationHelper
    {
        private const string ResourceId = "AppFramework.Localization.Resources.LocalTranslation";

        public static string Localize(string key)
        {
            return GetValue(key) ?? key;
        } 
        
        private static string GetValue(string key)
        {
            var locale = ContainerLocator.Container.Resolve<ILocale>();
            var cultureInfo = locale.GetCurrentCultureInfo();
            var resourceManager = new ResourceManager(ResourceId, typeof(LocalTranslationHelper).GetTypeInfo().Assembly);
            return resourceManager.GetString(key, cultureInfo);
        }
    }
}