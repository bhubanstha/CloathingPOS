using Autofac;
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
