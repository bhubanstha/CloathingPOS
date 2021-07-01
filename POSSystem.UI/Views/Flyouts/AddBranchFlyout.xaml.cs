using Autofac;
using MahApps.Metro.Controls;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;

namespace POSSystem.UI.Views.Flyouts
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddBranchFlyout : Flyout
    {
        private BranchViewModel model;
        public AddBranchFlyout()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<BranchViewModel>();// new CategoryViewModel();
            this.DataContext = model;
        }
    }
}
