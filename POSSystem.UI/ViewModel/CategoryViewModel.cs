using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        private Int64 _id;
        private string _name;
        private CategoryBO categoryBO;
        private List<Category> categories;

        private MetroWindow _window;


        public Int64 Id { 
            get { return _id; }
            set { _id = value;
                OnPropertyChanged();
            }
        }
        public string Name {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

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
        public ICommand DeleteCategoryCommand { get;  }

        public ICommand EditCategoryCommand { get; }
        public CategoryViewModel()
        {
            _window = Application.Current.MainWindow as MetroWindow;
            LoadCategories();
            CreateCategoryCommand = new DelegateCommand(SaveCategory);
            DeleteCategoryCommand = new DelegateCommand<Category>(DeleteCategory);
            EditCategoryCommand = new DelegateCommand<Category>(EditCategory);
        }

        private void EditCategory(Category obj)
        {
            if(obj != null)
            {
                this.Id = obj.Id;
                this.Name = obj.Name;
            }
        }

        private async void DeleteCategory(Category obj)
        {
            try
            {
                if (obj != null)
                {
                    categoryBO = new CategoryBO();
                    int i =  await categoryBO.Delete(obj.Id);
                    if (i > 0)
                    {
                        LoadCategories();
                    }
                }
            }
            catch (Exception ex)
            {
                await _window.ShowMessageAsync("Error", ex.Message, MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary, StaticContainer.DialogSettings);
            }
        }

        private async void SaveCategory()
        {
            categoryBO = new CategoryBO();
            Category c = new Category
            {
                Id = this.Id,
                Name = this.Name
            };

            int i = 0;
            if(this.Id>0)
            {
                i = await categoryBO.Update(c);
            }
            else
            {
                i = await categoryBO.Save(c);
            }
            
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
