using POS.BusinessRule;
using POS.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        private CategoryBO categoryBO;
        private List<Category> categories;
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateCategoryCommand { get; }
        public CategoryViewModel()
        {
            LoadCategories();
            CreateCategoryCommand = new DelegateCommand(SaveCategory);
        }

        private async void SaveCategory()
        {
            categoryBO = new CategoryBO();
            Category c = new Category
            {
                Id = this.Id,
                Name = this.Name
            };
            int i = await categoryBO.Save(c);
            if (i > 0)
            {
                this.Id = 0;
                this.Name = "";
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            categoryBO = new CategoryBO();
            Categories = categoryBO.GetCategories();
        }
    }
}
