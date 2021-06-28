using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace POSSystem.WPF.UI.Pages.Tiles
{
    /// <summary>
    /// Interaction logic for ServiceTile.xaml
    /// </summary>
    public partial class ServiceTile : UserControl
    {


        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public bool LoadImageFromResource
        {
            get { return (bool)GetValue(LoadImageFromResourceProperty); }
            set { SetValue(LoadImageFromResourceProperty, value); }
        }

        public string ServiceTitle
        {
            get { return (string)GetValue(ServiceTitleProperty); }
            set { SetValue(ServiceTitleProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }



        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }



        public Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageStretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageStretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ServiceTile), new FrameworkPropertyMetadata(Stretch.UniformToFill, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ImageStretchPropertyChange, null, false, UpdateSourceTrigger.PropertyChanged));





        // Using a DependencyProperty as the backing store for LoadImageFromResource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadImageFromResourceProperty =
            DependencyProperty.Register("LoadImageFromResource", typeof(bool), typeof(ServiceTile), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));



        // Using a DependencyProperty as the backing store for ServiceTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServiceTitleProperty =
            DependencyProperty.Register("ServiceTitle", typeof(string), typeof(ServiceTile),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ServiceTitlePropertyChange, null, false, UpdateSourceTrigger.PropertyChanged));



        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ServiceTile),
                 new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DescriptionPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));




        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ServiceTile), new PropertyMetadata(ImageSourcePropertyChanged));


        public ServiceTile()
        {
            InitializeComponent();
        }


        private static void ImageStretchPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ServiceTile st)
            {
                st.UpdateImageStretch();
            }
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

        private void UpdateImageStretch()
        {
            imgServiceImage.Stretch = ImageStretch;
        }

        private ImageSource GetImageFromPath(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                if (LoadImageFromResource)
                {
                    BitmapImage image = new BitmapImage();

                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/POSSystem.WPF.UI;component/" + imageName);
                    image.EndInit();

                    return image;
                }
                else
                {
                    if (File.Exists(imageName))
                    {
                        using (System.Drawing.Bitmap img = new System.Drawing.Bitmap(imageName))
                        {
                            return ImageSourceFromBitmap(img);
                        }
                    }

                }
            }
            return null;
        }


        public ImageSource ImageSourceFromBitmap(System.Drawing.Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
    }
}
