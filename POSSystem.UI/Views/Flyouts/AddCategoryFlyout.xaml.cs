using MahApps.Metro.Controls;
using POSSystem.UI.ViewModel;

namespace POSSystem.UI.Views.Flyouts
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
            model = new CategoryViewModel();
            this.DataContext = model;
        }
    }
}
