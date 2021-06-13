using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : UserControl
    {
        private InventoryViewModel model;
        public InventoryView()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<InventoryViewModel>();// new InventoryViewModel();
            this.DataContext = model;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
