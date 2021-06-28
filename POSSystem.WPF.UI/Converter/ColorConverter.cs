using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace POSSystem.WPF.UI.Converter
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorHex = value as string;
            Color? tempColor = MahApps.Metro.Controls.ColorHelper.ColorFromString(colorHex);

            Color color = tempColor.HasValue ? tempColor.Value : Color.FromScRgb(0, 255, 255, 255);
            SolidColorBrush brush = new SolidColorBrush(color);
            //System.Drawing.Color.Transparent;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
