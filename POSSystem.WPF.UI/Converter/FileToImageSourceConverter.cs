using POS.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;

namespace POSSystem.WPF.UI.Converter
{
    public class FileToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string imageName = value.ToString();
                string imagePath = FilePath.GetProfileImageFullPath(imageName);
                ImageSource image = ImageUtility.GetImageSource(imagePath);
                return image;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
