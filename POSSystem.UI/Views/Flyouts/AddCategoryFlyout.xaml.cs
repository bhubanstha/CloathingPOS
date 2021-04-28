using MahApps.Metro.Controls;
using POSSystem.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
