using Autofac;
using MahApps.Metro.Controls;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;

namespace POSSystem.UI.Views.Flyouts
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddBrandFlyout : Flyout
    {
        private CategoryViewModel model;
        public AddBrandFlyout()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<CategoryViewModel>();// new CategoryViewModel();
            this.DataContext = model;
        }
    }
}
