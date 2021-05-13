using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl
    {
        BitmapImage bitmapImage = null;
        public AboutView()
        {
            InitializeComponent();
            this.Loaded += AboutView_Loaded;
        }

        private void AboutView_Loaded(object sender, RoutedEventArgs e)
        {
            bitmapImage = TakeScreenshot();
            ThisWindowPic.Source = bitmapImage;
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

        private void newScreenshot()
        {
            // Store the size of the map control
            int Width = (int)MyMap.RenderSize.Width;
            int Height = (int)MyMap.RenderSize.Height;
            System.Windows.Point relativePoint = MyMap.TransformToAncestor(Application.Current.MainWindow).Transform(new System.Windows.Point(0, 0));
            int X = (int)relativePoint.X;
            int Y = (int)relativePoint.Y;

            Bitmap Screenshot = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(Screenshot);
            // snip wanted area
            G.CopyFromScreen(X, Y, 0, 0, new System.Drawing.Size(Width, Height), CopyPixelOperation.SourceCopy);

            string fileName = "C:\\myCapture.bmp";
            System.IO.FileStream fs = System.IO.File.Open(fileName, System.IO.FileMode.OpenOrCreate);
            Screenshot.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
            fs.Close();


            //Code Fixes

            RenderTargetBitmap renderTargetBitmap =
    new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(yourMapControl);
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (Stream fileStream = File.Create(filePath))
            {
                pngImage.Save(fileStream);
            }
        }
    }
}
