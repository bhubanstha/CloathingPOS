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
            _model = new SalesReturnViewModel();
            this.DataContext = _model;
        }
    }
}
