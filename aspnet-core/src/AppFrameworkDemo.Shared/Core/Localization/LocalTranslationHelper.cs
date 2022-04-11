﻿using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace AppFrameworkDemo.Shared.Localization
{
    public static class LocalTranslationHelper
    {
        private const string ResourceId = "AppFrameworkDemo.Shared.Core.Localization.Resources.LocalTranslation";

        public static string Localize(string key)
        {
            return GetValue(key) ?? key;
        }

        private static string GetValue(string key)
        {
            var locale = DependencyService.Get<ILocale>();
            var cultureInfo = locale.GetCurrentCultureInfo();
            var resourceManager = new ResourceManager(ResourceId, typeof(LocalTranslationHelper).GetTypeInfo().Assembly);
            return resourceManager.GetString(key, cultureInfo);
        }
    }
}