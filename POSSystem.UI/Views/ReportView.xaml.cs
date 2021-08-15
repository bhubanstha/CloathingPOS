using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            InitializeComponent();
            ReportViewModel vm = StaticContainer.Container.Resolve<ReportViewModel>();
            vm.PdfViewer = moonPdfPanel;
            DataContext = vm;
        }
    }
}
