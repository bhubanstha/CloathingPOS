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
        //public string ServiceTitle { get; set; }
        //public string ImageSource { get; set; }
        //public string Description { get; set; }





        public string ServiceTitle
        {
            get { return (string)GetValue(ServiceTitleProperty); }
            set { SetValue(ServiceTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ServiceTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServiceTitleProperty =
            DependencyProperty.Register("ServiceTitle", typeof(string), typeof(ServiceTile), 
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ServiceTitlePropertyChange, null, false, UpdateSourceTrigger.PropertyChanged));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ServiceTile),
                 new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DescriptionPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));



        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ServiceTile), new PropertyMetadata(ImageSourcePropertyChanged));

        

        public ServiceTile()
        {
            InitializeComponent();
            //this.Loaded += ServiceTile_Loaded;
        }

        private void ServiceTile_Loaded(object sender, RoutedEventArgs e)
        {
            imgServiceImage.Source = GetImageFromPath(ImageSource);
            lblServiceTitle.Content = ServiceTitle;
            lblServiceDescription.Text = Description;
        }


        private static void ServiceTitlePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ServiceTile st)
            {
                st.UpdateServiceTitle();
            }
        }

        private static void DescriptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ServiceTile st)
            {
                st.UpdateDescription();
            }
        }

        private static void ImageSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ServiceTile st)
            {
                st.UpdateImage();
            }
        }


        private void UpdateServiceTitle()
        {
            lblServiceTitle.Content = ServiceTitle;
        }

        

        private void UpdateDescription()
        {
            lblServiceDescription.Text = Description;
        }



        private void UpdateImage()
        {
            imgServiceImage.Source = GetImageFromPath(ImageSource);
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
