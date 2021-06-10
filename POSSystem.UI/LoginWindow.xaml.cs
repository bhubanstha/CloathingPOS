using Autofac;
using MahApps.Metro.Controls;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
using System.Windows;

namespace POSSystem.UI.Views
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        private LoginViewModel _viewModel;
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.LoginWindow = this;
            DataContext = _viewModel;
            this.Loaded += LoginWindow_Loaded;
            this.Closed += LoginWindow_Closed;
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var userName = Convert.ToString( Application.Current.Properties["UserName"] );
            var rememberMe = Convert.ToBoolean( Application.Current.Properties["RememberMe"]) ;

            _viewModel.LoginUser.UserName = userName;
            _viewModel.LoginUser.RememberMe = rememberMe;
        }

        private void LoginWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow = null;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ForgotPasswordWindow forgotPassword = StaticContainer.Container.Resolve<ForgotPasswordWindow>();
            Application.Current.MainWindow = forgotPassword;
            this.Close();
            forgotPassword.Show();
        }
    }
}
