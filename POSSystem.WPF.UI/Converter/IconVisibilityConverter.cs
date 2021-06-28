using POS.Core.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace POSSystem.WPF.UI.Converter
{
    public class IconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value == null )
            {
                return Visibility.Visible;
            }
            else if(!string.IsNullOrEmpty(value.ToString()))
            {
                string fullpath = FilePath.GetProfileImageFullPath(value.ToString());
                if (!File.Exists(fullpath))
                    return Visibility.Visible;
            }
           

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
