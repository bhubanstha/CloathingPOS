using Autofac;
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
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class InventoryViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private ObservableCollection<CategoryWrapper> _categories = null;
        private ObservableCollection<BrandWrapper> _brands = null;
        private InventoryBO InventoryBO;
        private CategoryBO CategoryBO ;
        private string _buttonText = "Create Inventory";

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

        

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; OnPropertyChanged(); }
        }




        public ICommand SaveCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand AddBrandCommand { get; }
        public ICommand ResetCommand { get; }

        public InventoryViewModel(ICacheService cacheService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CategoryBO = new CategoryBO();
            Inventory = new InventoryWrapper(new Inventory())
            {
                ColorNameEntryEnabled = false,
                FirstPurchaseDate = DateTime.Now,
                BranchId = StaticContainer.ActiveBranchId,
                Quantity = 1,
                PurchaseRate = 1,
                RetailRate = 1,
                Color = "#000000"//Default black colour
            };
            LoadCategories();
            LoadBrands();
            SaveCommand = new DelegateCommand(OnSaveProduct);
            AddCategoryCommand = new DelegateCommand(OnOpenAddCategoryFlyout);
            AddBrandCommand = new DelegateCommand(OnOpenAddBrandFlyout);
            ResetCommand = new DelegateCommand(OnReset);
            eventAggregator.GetEvent<CategoryChangedEvent>().Subscribe(ReLoadCategories);
            eventAggregator.GetEvent<BrandChangedEvent>().Subscribe(ReLoadBrands);
            eventAggregator.GetEvent<InventoryChangedEvent>().Subscribe(OnInventoryEditReceived);
        }

        private void OnReset()
        {
            
            Inventory.ColorNameEntryEnabled = false;
            Inventory.FirstPurchaseDate = DateTime.Now;
            Inventory.CategoryId = 1;
            Inventory.BrandId = 1;
            Inventory.Name = "";
            Inventory.Code = "";
            Inventory.BarCode = "";
            Inventory.Size = "";
            Inventory.Color = "";
            Inventory.ColorName = "";
            Inventory.Quantity = 1;
            Inventory.PurchaseRate = 1;
            Inventory.RetailRate = 1;
            ButtonText = "Create Inventory";
        }

        private void OnInventoryEditReceived(InventoryChangedEventArgs obj)
        {
            if(obj.Action == EventAction.Edit)
            {
                Inventory.Id = obj.Inventory.Id;
                Inventory.FirstPurchaseDate = obj.Inventory.FirstPurchaseDate;
                Inventory.PurchaseRate = obj.Inventory.PurchaseRate;
                Inventory.RetailRate = obj.Inventory.RetailRate;
                Inventory.Name = obj.Inventory.Name;
                Inventory.Size = obj.Inventory.Size;
                Inventory.Color = obj.Inventory.Color;
                Inventory.ColorName = obj.Inventory.ColorName;
                Inventory.Quantity = obj.Inventory.Quantity;
                Inventory.CategoryId = obj.Inventory.CategoryId;
                Inventory.CategoryName = obj.Inventory.Category.Name;
                Inventory.BrandId = obj.Inventory.BrandId;
                Inventory.BrandName = obj.Inventory.Brand.Name;
                Inventory.Code = obj.Inventory.Code;
                Inventory.BarCode = obj.Inventory.BarCode;
                Inventory.BranchId = obj.Inventory.BranchId;
                Inventory.UserId = obj.Inventory.UserId;
                StaticContainer.UIHamburgerMenuControl.SelectedIndex = 1;
                if (obj.Inventory.Branch != null)
                {
                    MainWindowViewModel model = StaticContainer.Container.Resolve<MainWindowViewModel>();
                    model.UpdateBranchOnEdit(obj.Inventory.BranchId.Value, obj.Inventory.Branch.BranchName);
                }
                ButtonText = "Update Inventory";
            }
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
            EventAction eventAction;
            string msg = "";
            try
            {
                Inventory inventory = new Inventory
                {
                    Id = this.Inventory.Id,
                    UserId = _loggedInUser.Id,
                    CategoryId = this.Inventory.CategoryId,
                    BrandId = this.Inventory.BrandId,
                    BranchId = StaticContainer.ActiveBranchId,
                    Color = this.Inventory.Color,
                    FirstPurchaseDate = this.Inventory.FirstPurchaseDate,
                    Name = this.Inventory.Name,
                    PurchaseRate = this.Inventory.PurchaseRate,
                    Quantity = this.Inventory.Quantity,
                    RetailRate = this.Inventory.RetailRate,
                    Size = this.Inventory.Size,
                    ColorName = this.Inventory.ColorName,
                    Code = this.Inventory.Code,
                    BarCode = this.Inventory.Code.PadRight(8,'0').Substring(0,8) + Guid.NewGuid().ToString("N")
                };
                InventoryBO = new InventoryBO();
                int c = 0;
                if(this.Inventory.Id>0)
                {
                    c = await InventoryBO.UpdateInventory(inventory);
                    eventAction = EventAction.Update;
                    msg = "updated";
                }
                else
                {
                    c = await InventoryBO.Save(inventory);
                    eventAction = EventAction.Add;
                    msg = "added into inventory";
                }
                
                if (c > 0)
                {
                    InventoryChangedEventArgs args = new InventoryChangedEventArgs
                    {
                        Inventory = inventory,
                        Action = eventAction
                    };
                    _eventAggregator.GetEvent<InventoryChangedEvent>().Publish(args);
                    ResetInventory();

                    StaticContainer.ShowNotification("Product Added", $"Product: {inventory.Name} - {inventory.Size} purchased on {inventory.FirstPurchaseDate.ToString("yyyy/MM/dd")} {msg}.", NotificationType.Success);

                }
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
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
            ButtonText = "Create Inventory";
        }
    }
}
