using MahApps.Metro.Controls;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class InventoryViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<CategoryWrapper> _categories = null;
        public virtual ObservableCollection<CategoryWrapper> Categories 
        { 
            get { return _categories; }
            set 
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        public InventoryWrapper Inventory { get; set; }

        private InventoryBO InventoryBO;
        private CategoryBO CategoryBO;


        public ICommand SaveCommand { get; }
        public ICommand AddCategoryCommand { get; }

        public InventoryViewModel(IEventAggregator eventAggregator)
        {

            CategoryBO = new CategoryBO();
            Inventory = new InventoryWrapper(new Inventory())
            {
                ColorNameEntryEnabled = false,
                FirstPurchaseDate = DateTime.Now
            };
            LoadCategories();
            SaveCommand = new DelegateCommand(SaveProduct);
            AddCategoryCommand = new DelegateCommand(OpenAddCategoryFlyout);

            eventAggregator.GetEvent<CategoryChangedEvent>().Subscribe(ReLoadCategories);
        }

        private void OpenAddCategoryFlyout()
        {
            Flyout f = StaticContainer.AddCategoryFlyout;
            f.IsOpen = !f.IsOpen;
        }

        private async void SaveProduct()
        {
            try
            {
                Inventory inventory = new Inventory
                {
                    Id = this.Inventory.Id,
                    CategoryId = this.Inventory.CategoryId,
                    Color = this.Inventory.Color,
                    FirstPurchaseDate = this.Inventory.FirstPurchaseDate,
                    Name = this.Inventory.Name,
                    PurchaseRate = this.Inventory.PurchaseRate,
                    Quantity = this.Inventory.Quantity,
                    RetailRate = this.Inventory.RetailRate,
                    Size = this.Inventory.Size,
                    ColorName = this.Inventory.ColorName
                };
                InventoryBO = new InventoryBO();
                int c = await InventoryBO.Save(inventory);
                if (c > 0)
                {
                    StaticContainer.ShowNotification("Product Added", $"Product: {inventory.Name} - {inventory.Size} purchased on {inventory.FirstPurchaseDate.ToString("yyyy/MM/dd")} added into inventory.", NotificationType.Success);

                }
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", "Could not create inventory. Please provide all the required item information.", NotificationType.Error);
            }
        }

        private void LoadCategories()
        {
            List<Category> categories = CategoryBO.GetCategories();
            Categories = new ObservableCollection<CategoryWrapper>();
            foreach (Category item in categories)
            {
                CategoryWrapper wrapper = new CategoryWrapper(item);
                Categories.Add(wrapper);
            }
        }

        private void ReLoadCategories(CategoryChangedEventArgs args)
        {
            if(args.Action == EventAction.Add)
            {
                CategoryWrapper wrapper = new CategoryWrapper(args.Category);
                Categories.Add(wrapper);
            }
            else if (args.Action == EventAction.Remove)
            {
                var itemToRemove = Categories.Where(x => x.Id == args.Category.Id).FirstOrDefault();
                Categories.Remove(itemToRemove);
            }
            else if (args.Action == EventAction.Update)
            {
                var itm = Categories.Where(x => x.Id == args.Category.Id).FirstOrDefault();
                itm.Name = args.Category.Name;
                OnPropertyChanged("Categories");
            }

        }
    }
}
