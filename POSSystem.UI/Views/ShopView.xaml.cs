using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for ShopView.xaml
    /// </summary>
    public partial class ShopView : UserControl
    {
        private ShopViewModel model;
        public ShopView()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<ShopViewModel>();
            this.DataContext = model;
        }
    }
}
