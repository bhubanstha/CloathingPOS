using Autofac;
using MahApps.Metro.Controls;
using POSSystem.WPF.UI.Service;
using POSSystem.WPF.UI.ViewModel;

namespace POSSystem.WPF.UI.Views.Flyouts
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddBrandFlyout : Flyout
    {
        private BrandViewModel model;
        public AddBrandFlyout()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<BrandViewModel>();// new CategoryViewModel();
            this.DataContext = model;
        }
    }
}
