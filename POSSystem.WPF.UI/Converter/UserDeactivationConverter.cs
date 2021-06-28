﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace POSSystem.WPF.UI.Converter
{
    public class UserDeactivationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
