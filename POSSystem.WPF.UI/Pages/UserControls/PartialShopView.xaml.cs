using POSSystem.WPF.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.WPF.UI.Views
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
