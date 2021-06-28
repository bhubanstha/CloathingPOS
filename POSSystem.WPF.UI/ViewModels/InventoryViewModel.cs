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

namespace POSSystem.WPF.UI.ViewModel
{
    public class InventoryViewModel : NotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;
        private ObservableCollection<CategoryWrapper> _categories = null;
        private ObservableCollection<BrandWrapper> _brands = null;
        public virtual ObservableCollection<CategoryWrapper> Categories 
        { 
            get { return _categories; }
            set 
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public virtual ObservableCollection<BrandWrapper> Brands
        {
            get { return _brands; }
            set
            {
                _brands = value;
                OnPropertyChanged();
            }
        }


        public InventoryWrapper Inventory { get; set; }

        private InventoryBO InventoryBO;
        private CategoryBO CategoryBO;


        public ICommand SaveCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand AddBrandCommand { get; }

        public InventoryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CategoryBO = new CategoryBO();
            Inventory = new InventoryWrapper(new Inventory())
            {
                ColorNameEntryEnabled = false,
                FirstPurchaseDate = DateTime.Now
            };
            LoadCategories();
            LoadBrands();
            SaveCommand = new DelegateCommand(OnSaveProduct);
            AddCategoryCommand = new DelegateCommand(OnOpenAddCategoryFlyout);
            AddBrandCommand = new DelegateCommand(OnOpenAddBrandFlyout);
            eventAggregator.GetEvent<CategoryChangedEvent>().Subscribe(ReLoadCategories);
            eventAggregator.GetEvent<BrandChangedEvent>().Subscribe(ReLoadBrands);
        }

        private void OnOpenAddCategoryFlyout()
        {
            Flyout f = StaticContainer.AddCategoryFlyout;
            f.IsOpen = !f.IsOpen;
        }

        private void OnOpenAddBrandFlyout()
        {
            Flyout f = StaticContainer.AddBrandFlyout;
            f.IsOpen = !f.IsOpen;
        }

        private async void OnSaveProduct()
        {
            try
            {
                Inventory inventory = new Inventory
                {
                    Id = this.Inventory.Id,
                    CategoryId = this.Inventory.CategoryId,
                    BrandId = this.Inventory.BrandId,
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
                    InventoryChangedEventArgs args = new InventoryChangedEventArgs
                    {
                        Inventory = inventory,
                        Action = EventAction.Add
                    };
                    _eventAggregator.GetEvent<InventoryChangedEvent>().Publish(args);
                    ResetInventory();

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

        private void LoadBrands()
        {
            BrandBO brandBo = new BrandBO();
            List<Brand> brands = brandBo.GetBrands();
            Brands = new ObservableCollection<BrandWrapper>();
            foreach (Brand item in brands)
            {
                BrandWrapper wrapper = new BrandWrapper(item);
                Brands.Add(wrapper);
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

        private void ReLoadBrands(BrandChangedEventArgs args)
        {
            if (args.Action == EventAction.Add)
            {
                BrandWrapper wrapper = new BrandWrapper(args.Brand);
                Brands.Add(wrapper);
            }
            else if (args.Action == EventAction.Remove)
            {
                var itemToRemove = Brands.Where(x => x.Id == args.Brand.Id).FirstOrDefault();
                Brands.Remove(itemToRemove);
            }
            else if (args.Action == EventAction.Update)
            {
                var itm = Brands.Where(x => x.Id == args.Brand.Id).FirstOrDefault();
                itm.Name = args.Brand.Name;
                OnPropertyChanged("Categories");
            }

        }

        private void ResetInventory()
        {
            Inventory.Id = 0;
            Inventory.CategoryId = 0;
            Inventory.Color = "";
            Inventory.FirstPurchaseDate = DateTime.Now;
            Inventory.Name = "";
            Inventory.PurchaseRate = 1;
            Inventory.Quantity = 1;
            Inventory.RetailRate = 1;
            Inventory.Size = "";
            Inventory.ColorName = "";
        }
    }
}
