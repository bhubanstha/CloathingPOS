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
    public class ControlHightCal : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double controlHeight = 0;
            Window win = Application.Current.MainWindow;
            System.Windows.Forms.Screen sc = GetActiveScreen(ref win);
            double winHeight = sc.WorkingArea.Height;
            double.TryParse(parameter.ToString(), out controlHeight);
            return winHeight - controlHeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private System.Windows.Forms.Screen GetActiveScreen(ref Window appWindow)
        {
            try
            {
                System.Windows.Forms.Screen sc = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(appWindow).Handle);
                return sc;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
