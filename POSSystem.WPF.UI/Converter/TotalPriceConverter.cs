using System;
using System.Globalization;
using System.Windows.Data;

namespace POSSystem.WPF.UI.Converter
{
    public class TotalPriceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int qty = 0;
            decimal rate = 0m;
            decimal discount = 0m;

            Int32.TryParse(values[0].ToString(), out qty);
            decimal.TryParse(values[1].ToString(), out rate);
            decimal.TryParse(values[2].ToString(), out discount);

            return (qty * rate) - discount;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
