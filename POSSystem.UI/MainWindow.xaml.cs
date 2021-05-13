﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
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

namespace POSSystem.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel _model;
        public IDialogCoordinator DialogCoordinator;
        public MainWindow(MainWindowViewModel model)
        {
            InitializeComponent();
            _model = model;
            _model.Window = this;
            _model.SettingFlyout = this.SettingsFlyout;
            DialogCoordinator = _model._dialogCoordinator;
            DataContext = _model;
            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //string msg = string.Format("{0},{1},{2}", container.ActualWidth, PrimaryScrollViewer.Width, PrimaryGrid.Width);
            //MessageBox.Show(msg, "Width");
            //PrimaryScrollViewer.Width = PrimaryGrid.Width = container.ActualWidth;
            //StaticContainer.AppScreenshot = Screenshot();
            System.Drawing.Size s = new System.Drawing.Size();
            s.Width = (int)container.RenderSize.Width;
            s.Height = (int)container.RenderSize.Height;

            Task.Delay(new TimeSpan(0, 0, 1)).ContinueWith(o => { Screenshot(s, container, this); });
        }

        private void HamburgerMenuControl_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            HamburgerMenuControl.Content = args.InvokedItem;
        }

        private void Screenshot(System.Drawing.Size size, Canvas canvas, Window window)
        {

            // Store the size of the map control
            //int Width = (int)canvas.RenderSize.Width;
            //int Height = (int)canvas.RenderSize.Height;
            //System.Windows.Point relativePoint = new System.Windows.Point(0, 0);
           
            this.Dispatcher.Invoke(() =>
            {
                BitmapImage image = new BitmapImage();
                System.Windows.Point relativePoint =  canvas
                .TransformToAncestor(window)
                .Transform(new System.Windows.Point(0, 0));

                int X = (int)relativePoint.X;
                int Y = (int)relativePoint.Y;
               

                using (Bitmap Screenshot = new Bitmap(size.Width, size.Height))
                {
                    using (Graphics G = Graphics.FromImage(Screenshot))
                    {
                        // snip wanted area
                        G.CopyFromScreen(X, Y, 0, 0, new System.Drawing.Size(size.Width, size.Height), CopyPixelOperation.SourceCopy);
                        RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(size.Width, size.Height, 96, 96, PixelFormats.Pbgra32);
                        renderTargetBitmap.Render(canvas);

                        PngBitmapEncoder pngImage = new PngBitmapEncoder();
                        pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                        using (var ms = new MemoryStream())
                        {
                            pngImage.Save(ms);
                            ms.Seek(0, SeekOrigin.Begin);

                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();
                        }


                    }
                }
                StaticContainer.AppScreenshot = image;
            });
            
        }
    }
}
