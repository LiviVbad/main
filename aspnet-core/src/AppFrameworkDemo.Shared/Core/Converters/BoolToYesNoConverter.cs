﻿using AppFrameworkDemo.Shared.Localization;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppFrameworkDemo.Shared.Converters
{
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
                return booleanValue ? Local.Localize("Yes") : Local.Localize("No");

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}