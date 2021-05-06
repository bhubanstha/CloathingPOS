using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.UI.ViewModel;
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
        }

        private void HamburgerMenuControl_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            HamburgerMenuControl.Content = args.InvokedItem;
        }
    }
}
