using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for UserProfileView.xaml
    /// </summary>
    public partial class UserProfileView : UserControl
    {
        private UserProfileViewModel _model;
        public UserProfileView()
        {
            InitializeComponent();
            _model = StaticContainer.Container.Resolve<UserProfileViewModel>();
            this.DataContext = _model;
        }
    }
}
