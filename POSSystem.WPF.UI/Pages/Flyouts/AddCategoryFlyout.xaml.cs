using Autofac;
using MahApps.Metro.Controls;
using POSSystem.WPF.UI.Service;
using POSSystem.WPF.UI.ViewModel;

namespace POSSystem.WPF.UI.Views.Flyouts
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategoryFlyout : Flyout
    {
        private CategoryViewModel model;
        public AddCategoryFlyout()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<CategoryViewModel>();// new CategoryViewModel();
            this.DataContext = model;
        }
    }
}
