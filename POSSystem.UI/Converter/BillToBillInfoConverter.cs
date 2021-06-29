using POS.BusinessRule;
using POS.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace POSSystem.UI.Converter
{
    public class BillToBillInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                BillBO billBO = new BillBO();
                Bill b = billBO.GetById(System.Convert.ToInt64(value));
                return $"Bill No.: {b.Id} - {b.BillTo} - ({b.BillingAddress})";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
