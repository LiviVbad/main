﻿using System;
using System.Globalization;
using AppFramework.ApiClient;
using AppFramework.Extensions;
using Prism.Ioc;

namespace AppFramework.Localization
{
    public static class Local
    {
        public static string Localize(string text)
        {
            return LocalizeInternal(text);
        }

        public static string Localize(string text, params object[] args)
        {
            return string.Format(LocalizeInternal(text), args);
        }

        public static string LocalizeWithThreeDots(string text, params object[] args)
        {
            var localizedText = Localize(text, args);
            return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? "..." + localizedText : localizedText + "...";
        }

        public static string LocalizeWithParantheses(string text, object valueWithinParentheses, params object[] args)
        {
            var localizedText = Localize(text);
            return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft
                ? " (" + valueWithinParentheses + ")" + localizedText
                : localizedText + " (" + valueWithinParentheses + ")";
        }

        private static string LocalizeInternal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var appContext = ContainerLocator.Container.Resolve<IApplicationContext>();
            if (appContext.Configuration == null)
                return text;

            return appContext.Configuration.Localization.Localize(text);
        }
    }
}