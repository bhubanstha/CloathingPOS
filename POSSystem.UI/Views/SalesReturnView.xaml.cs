using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for SalesReturnView.xaml
    /// </summary>
    public partial class SalesReturnView : UserControl
    {
        private SalesReturnViewModel _model;
        public SalesReturnView()
        {
            InitializeComponent();
            _model = StaticContainer.Container.Resolve<SalesReturnViewModel>();
            this.DataContext = _model;
        }
    }
}
