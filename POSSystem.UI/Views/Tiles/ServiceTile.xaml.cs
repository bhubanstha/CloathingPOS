using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSSystem.UI.Views.Tiles
{
    /// <summary>
    /// Interaction logic for ServiceTile.xaml
    /// </summary>
    public partial class ServiceTile : UserControl
    {
        public string ServiceTitle { get; set; }
        public string ImageSource { get; set; }
        public string Description { get; set; }


        public ServiceTile()
        {
            InitializeComponent();
            this.Loaded += ServiceTile_Loaded;
        }

        private void ServiceTile_Loaded(object sender, RoutedEventArgs e)
        {
            imgServiceImage.Source = GetImageFromPath(ImageSource);
            lblServiceTitle.Content = ServiceTitle;
            lblServiceDescription.Text = Description;
        }

        private ImageSource GetImageFromPath(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                BitmapImage image = new BitmapImage();

                image.BeginInit();
                image.UriSource = new Uri("pack://application:,,,/POSSystem.UI;component/" + imageName);
                image.EndInit();

                return image;
            }
            return null;
        }
    }
}
