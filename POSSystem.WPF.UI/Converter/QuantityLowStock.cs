using System;
using System.Globalization;
using System.Windows.Data;

namespace POSSystem.WPF.UI.Converter
{
    public class QuantityLowStock : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var qty = 0;
            Int32.TryParse(value.ToString(), out qty);
            return qty < 5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
