using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace POSSystem.WPF.UI.Converter
{
    public class ControlHeightCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double controlHeight = 0;
            Window win =  Application.Current.MainWindow;
            //System.Windows.Forms.Screen sc = GetActiveScreen(ref win);
            double winHeight = win.Height;
            double.TryParse(values[0].ToString(), out controlHeight);
            return winHeight - controlHeight;



            //foreach (var val in values)
            //{
            //    double height = (double)val;
            //    if(height == double.PositiveInfinity || (height == 0 && (double)values[0] == 0))
            //    {
            //        height = Application.Current.MainWindow.Height;
            //    }
            //    controlHeight = controlHeight == 0 ? height : controlHeight - height;
            //}
            //return controlHeight;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
    }
}
