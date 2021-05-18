using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

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
            _viewModel.Window = this;
            DataContext = _viewModel;


        }


        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            Application.Current.MainWindow = forgotPassword;
            this.Close();
            forgotPassword.Show();
        }
    }
}
