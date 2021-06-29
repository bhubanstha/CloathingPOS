using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for SalesListView.xaml
    /// </summary>
    public partial class SalesListView : UserControl
    {
        public SalesListView()
        {
            InitializeComponent();
            this.DataContext = StaticContainer.Container.Resolve<SalesListViewModel>();
        }
    }
}
