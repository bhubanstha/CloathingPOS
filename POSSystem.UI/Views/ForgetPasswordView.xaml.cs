using Autofac;
using Notifications.Wpf;
using POSSystem.UI.Service;
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

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for ForgetPasswordView.xaml
    /// </summary>
    public partial class ForgetPasswordView : UserControl
    {
        private bool _enableUserName = false;

        public bool EnableUserName
        {
            get { return _enableUserName ; }
            set { _enableUserName =  value; }
        }


        private ForgetPasswordViewModel _model;
        public bool ShowBackButton { get; set; }


        public ForgetPasswordView()
        {
            InitializeComponent();
            ICacheService cacheService = StaticContainer.Container.Resolve<ICacheService>();
            _model = new ForgetPasswordViewModel(cacheService);
            _model.IsUserNameEditable = EnableUserName;
            this.DataContext = _model;
            this.Loaded += ForgetPasswordView_Loaded;
        }

        private void ForgetPasswordView_Loaded(object sender, RoutedEventArgs e)
        {
            btnBackToLogin.Visibility = ShowBackButton ? Visibility.Visible : Visibility.Hidden;
        }

        private void txtConfirmPassword_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void ButtonBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //StaticContainer.NotificationManager.Show(new NotificationContent
                //{
                //    Title = "Back to Login",
                //    Message = "Redirecting to Login windows",
                //    Type = NotificationType.Information
                //});

                LoginWindow loginWindow = StaticContainer.Container.Resolve<LoginWindow>();
                Application.Current.MainWindow = loginWindow;

                var currentWindow = Window.GetWindow(this);
                currentWindow.Close();
                loginWindow.Show();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
