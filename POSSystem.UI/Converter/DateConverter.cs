﻿using POSSystem.UI.Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace POSSystem.UI.Converter
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt;
            DateTime.TryParse(value.ToString(), out dt);
            NepDate nepDate =  NepDateConverter.EngToNep(dt);
            return nepDate.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
