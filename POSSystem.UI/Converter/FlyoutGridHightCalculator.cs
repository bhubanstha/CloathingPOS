using System;
using System.Globalization;
using System.Windows.Data;

namespace POSSystem.UI.Converter
{
    public class FlyoutGridHightCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int windowHeight = 0;
            int otherControlHeight = 0;
            Int32.TryParse(values[0].ToString(), out windowHeight);
            Int32.TryParse(values[1].ToString(), out otherControlHeight);

            return windowHeight - otherControlHeight;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
