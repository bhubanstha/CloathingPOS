using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for CreateUserView.xaml
    /// </summary>
    public partial class CreateUserView : UserControl
    {
        public CreateUserView()
        {
            InitializeComponent();

            UserViewModel viewModel = new UserViewModel();
            viewModel.PasswordTextBox = txtPassword;
            this.DataContext = viewModel;

        }
    }
}
