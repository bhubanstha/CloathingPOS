using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using POS.Utilities;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            model.CheckUserIsAdmin();
            InitializeComponent();
            _model = model;
            _model.MainWindow = this;
            _model.SettingFlyout = this.SettingsFlyout;
            DataContext = _model;            
            this.Loaded += MainWindow_Loaded;      
        }

        private void GlobalElements()
        {
            StaticContainer.SettingFlyout = this.SettingsFlyout;// container.Resolve<SettingView>();
            StaticContainer.AddCategoryFlyout = this.CategoryFlyout;
            StaticContainer.AddBrandFlyout = this.BrandFlyout;
            StaticContainer.AddBranchFlyout = this.BranchFlyout;
            StaticContainer.NoSearchResultFlyout = this.NoSearchResultFlyout;
            StaticContainer.DialogCoordinator = this.DialogCoordinator;
            StaticContainer.UIHamburgerMenuControl = this.HamburgerMenuControl;
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalElements();
            
            double additionalWidth = btnCmdUserName.ActualWidth > 42 ? (btnCmdUserName.ActualWidth - 42) / 2 : btnCmdUserName.ActualWidth - 42;//42 is default size for button
            _model.PopupRightMargin = 120 + additionalWidth;
            //System.Drawing.Size s = new System.Drawing.Size();
            //s.Width = (int)container.RenderSize.Width;
            //s.Height = (int)container.RenderSize.Height;
            //Task.Delay(new TimeSpan(0, 0, 1)).ContinueWith(o => { Screenshot(s, container, this); });

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = this;
            StaticContainer.ThisApp.MainWindow = this;

            menuUserMgmt.IsVisible = _model.IsAdminMenuVisible;
            menuGraphs.IsVisible = _model.IsAdminMenuVisible;
            menuReport.IsVisible = _model.IsAdminMenuVisible;
            ShopView.IsVisible = _model.IsSysAdminMenuVisible;

            if (!_model.IsAdminMenuVisible)
            {
                HamburgerMenuControl.SelectedIndex = 1;
            }
            //App.Current.Resources["UserImage"] = ImageUtility.GetImageSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "InvoiceSample.png"));
        }

        private void HamburgerMenuControl_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            HamburgerMenuControl.Content = args.InvokedItem;
            //((MahApps.Metro.Controls.HamburgerMenuItemBase)args.InvokedItem).IsVisible = false;
        }

        private void Screenshot(System.Drawing.Size size, Panel canvas, Window window)
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
               // StaticContainer.AppScreenshot = image;
            });
            
        }
    }
}
