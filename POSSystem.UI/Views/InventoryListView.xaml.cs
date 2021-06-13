using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for InventoryList.xaml
    /// </summary>
    public partial class InventoryListView : UserControl
    {
        private InventoryListViewModel model;
        public InventoryListView()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<InventoryListViewModel>();
            this.DataContext = model;
        }
    }
}
