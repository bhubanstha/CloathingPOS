using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace POSSystem.UI.Converter
{
    public class ControlHeightCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double controlHeight = 0;
            foreach (var val in values)
            {
                double height = (double)val;
                if(height == double.PositiveInfinity || (height == 0 && (double)values[0] == 0))
                {
                    height = Application.Current.MainWindow.Height;
                }
                controlHeight = controlHeight == 0 ? height : controlHeight - height;
            }
            return controlHeight;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
