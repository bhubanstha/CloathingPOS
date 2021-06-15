using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for PartialShopView.xaml
    /// </summary>
    public partial class PartialShopView : UserControl
    {
        private SettingViewModel model;
        public PartialShopView()
        {
            InitializeComponent();
            model = new SettingViewModel();
            this.DataContext = model;
        }
    }
}
