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
            model = new InventoryListViewModel();
            this.DataContext = model;
        }
    }
}
