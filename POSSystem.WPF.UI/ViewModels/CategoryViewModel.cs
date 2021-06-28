using Notifications.Wpf.Core;
using POS.Core.BusinessRule;
using POS.Core.Model;
using POSSystem.WPF.UI.Event;
using POSSystem.WPF.UI.Service;
using POSSystem.WPF.UI.UIModel;
using POSSystem.WPF.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.WPF.UI.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private CategoryBO categoryBO;
        private ObservableCollection<CategoryWrapper> categories;

        public CategoryWrapper NewCategory { get; set; }
        public ObservableCollection<CategoryWrapper> Categories
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
        public ICommand ResetCommand { get; }
        public CategoryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            NewCategory = new CategoryWrapper(new Category());
            LoadCategories();
            CreateCategoryCommand = new DelegateCommand(OnSaveCategory);
            DeleteCategoryCommand = new DelegateCommand<CategoryWrapper>(OnDeleteCategory);
            EditCategoryCommand = new DelegateCommand<CategoryWrapper>(OnEditCategory);
            ResetCommand = new DelegateCommand(ResetEdit);
        }


        private void OnEditCategory(CategoryWrapper obj)
        {
            if(obj != null)
            {
                this.NewCategory.Id = obj.Id;
                this.NewCategory.Name = obj.Name;
            }
        }

        private async void OnDeleteCategory(CategoryWrapper obj)
        {
            try
            {
                if (obj != null)
                {
                    categoryBO = new CategoryBO();
                    int i =  await categoryBO.Delete(obj.Id);
                    if (i > 0)
                    {
                        RemoveCategoryFromList(obj.Model);
                        CategoryChangedEventArgs categoryChangedEventArgs = new CategoryChangedEventArgs
                        {
                            Category = obj.Model,
                            Action = EventAction.Remove
                        };
                        _eventAggregator.GetEvent<CategoryChangedEvent>().Publish(categoryChangedEventArgs);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
        }

        private async void OnSaveCategory()
        {
            try
            {
                categoryBO = new CategoryBO();
                Category c = new Category
                {
                    Id = NewCategory.Id,
                    Name = NewCategory.Name
                };

                CategoryChangedEventArgs categoryChangedEventArgs = await ManageCategory(c, categoryBO);  
                
                string msg = categoryChangedEventArgs.Action == EventAction.Add? 
                    "New Category Added Successfully." 
                    : "Category Changed Successfully.";

                _eventAggregator.GetEvent<CategoryChangedEvent>().Publish(categoryChangedEventArgs);
                StaticContainer.ShowNotification("Category", msg, NotificationType.Success);
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
            finally
            {
                ResetEdit();
            }
        }

        private async Task<CategoryChangedEventArgs> ManageCategory(Category category, CategoryBO bO)
        {
            CategoryChangedEventArgs categoryChangedEventArgs = new CategoryChangedEventArgs
            {
                Category = category
            };
            if (category.Id>0)
            {
                await bO.Update(category);
                categoryChangedEventArgs.Action = EventAction.Update;
            }
            else
            {
                await bO.Save(category);
                categoryChangedEventArgs.Action = EventAction.Add;
            }
            ManageCategoryInList(categoryChangedEventArgs);
            return categoryChangedEventArgs;
        }

        private void LoadCategories()
        {
            categoryBO = new CategoryBO();
            var categoryList = categoryBO.GetCategories();
            Categories = new ObservableCollection<CategoryWrapper>();
            foreach (Category category in categoryList)
            {
                CategoryWrapper categoryWrapper = new CategoryWrapper(category);
                Categories.Add(categoryWrapper);
            }
        }

        private void ManageCategoryInList(CategoryChangedEventArgs args)
        {
            if(args.Action == EventAction.Add)
            {
                AddNewCategoryToList(args.Category);
            }
            else if(args.Action == EventAction.Update)
            {
                UpdateCategoryInList(args.Category);
            }
            else
            {
                RemoveCategoryFromList(args.Category);
            }
        }

        private void AddNewCategoryToList(Category obj)
        {
            CategoryWrapper categoryWrapper = new CategoryWrapper(obj);
            Categories.Add(categoryWrapper);
        }

        private void UpdateCategoryInList(Category obj)
        {
            var item = Categories.Where(x => x.Id == obj.Id).FirstOrDefault();
            item.Name = obj.Name;
        }

        private void RemoveCategoryFromList(Category obj)
        {
            var item = Categories.Where(x => x.Id == obj.Id).FirstOrDefault();
            Categories.Remove(item);
        }
        


        private void ResetEdit()
        {
            this.NewCategory.Name = "";
            this.NewCategory.Id = 0;
        }

    }
}
