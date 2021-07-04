using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            _model = StaticContainer.Container.Resolve<ForgetPasswordViewModel>();           
            this.DataContext = _model;
            this.Loaded += ForgetPasswordView_Loaded;
        }

        private void ForgetPasswordView_Loaded(object sender, RoutedEventArgs e)
        {
            btnBackToLogin.Visibility = ShowBackButton ? Visibility.Visible : Visibility.Hidden;
            _model.IsUserNameEditable = EnableUserName;
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
