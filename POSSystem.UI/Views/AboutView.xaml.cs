using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl
    {
        BitmapImage bitmapImage = null;
        AppDetails appDetails;
        public AboutView()
        {
            InitializeComponent();
            appDetails = new AppDetails();
            this.DataContext = appDetails;
            this.Loaded += AboutView_Loaded;
        }

        private void AboutView_Loaded(object sender, RoutedEventArgs e)
        {
            //bitmapImage = StaticContainer.AppScreenshot;
            //ThisWindowPic.Source = bitmapImage;
        }

        private BitmapImage TakeScreenshot()
        {
            try
            {

                double screenLeft = SystemParameters.VirtualScreenLeft;
                double screenTop = SystemParameters.VirtualScreenTop;
                double screenWidth = SystemParameters.VirtualScreenWidth;
                double screenHeight = SystemParameters.VirtualScreenHeight;

                using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {


                        //String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                        //Opacity = .0;
                        g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);

                        using (var ms = new MemoryStream())
                        {
                            bmp.Save(ms, ImageFormat.Png);

                            BitmapImage bitImage = new BitmapImage();
                            ms.Seek(0, SeekOrigin.Begin);
                            bitImage.BeginInit();
                            bitImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitImage.StreamSource = ms;
                            bitImage.EndInit();
                            return bitImage;
                        }
                    }
                }

                //bmp.Save("C:\\Screenshots\\" + filename);
                //Opacity = 1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }

    class AppDetails : ViewModelBase
    {
        private string _appName;

        public string AppName
        {
            get { return _appName; }
            set { _appName = value; OnPropertyChanged(); }
        }


        public AppDetails()
        {
            AppName = StaticContainer.ApplicationName;
        }
    }
}
