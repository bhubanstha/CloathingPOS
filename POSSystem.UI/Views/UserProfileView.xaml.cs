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
            var cacheService = StaticContainer.Container.Resolve<ICacheService>();
            _model = new UserProfileViewModel(cacheService);
            this.DataContext = _model;
        }
    }
}
