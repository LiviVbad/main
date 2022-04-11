using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace AppFrameworkDemo.Localization
{
    public static class AppFrameworkDemoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AppFrameworkDemoConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AppFrameworkDemoLocalizationConfigurer).GetAssembly(),
                        "AppFrameworkDemo.Localization.AppFrameworkDemo"
                    )
                )
            );
        }
    }
}