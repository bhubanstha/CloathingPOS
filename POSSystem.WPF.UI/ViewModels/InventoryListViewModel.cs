using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.Core.BusinessRule;
using POS.Core.Model;
using POSSystem.WPF.UI.Event;
using POSSystem.WPF.UI.Pages.Dialog;
using POSSystem.WPF.UI.Service;
using POSSystem.WPF.UI.UIModel;
using POSSystem.WPF.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POSSystem.WPF.UI.ViewModel
{
    public class InventoryListViewModel : ViewModelBase
    {
        private InventoryBO _inventoryBo;
        MetroWindow _window;
        private UpdateInventoryDialog _updateInventoryDialog;
        private IEventAggregator _eventAggregator;

        private ObservableCollection<InventoryWrapper> _inventory;

        public ObservableCollection<InventoryWrapper> Inventory
        {
            get { return _inventory; }
            set 
            {
                _inventory = value;                 
                OnPropertyChanged(); 
            }
        }


        public ICommand DeleteInventoryItemCommand { get; }
        public ICommand AddInventoryItemStockCommand { get; }

        public InventoryListViewModel(UpdateInventoryDialog updateInventoryDialog, IEventAggregator eventAggregator)
        {
            _updateInventoryDialog = updateInventoryDialog;
            _eventAggregator = eventAggregator;
            _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            DeleteInventoryItemCommand = new DelegateCommand<InventoryWrapper>(DeleteInventoryItem);
            AddInventoryItemStockCommand = new DelegateCommand<InventoryWrapper>(OnAddInventoryItemStock);
            eventAggregator.GetEvent<InventoryChangedEvent>().Subscribe(ReloadInventory);
            LoadInventory();
        }

        private void OnAddInventoryItemStock(InventoryWrapper obj)
        {
            try
            {
                _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
                StaticContainer.InventoryUpdateDialog = _updateInventoryDialog;
                _eventAggregator.GetEvent<InventoryUpdatePopupOpenEvent>().Publish(obj.Model);
                _window.ShowMetroDialogAsync(_updateInventoryDialog);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        private void DeleteInventoryItem(InventoryWrapper obj)
        {
            _inventoryBo = new InventoryBO();
            _inventoryBo.RemoveItem(obj.Model);
            RemoveItem(obj.Model);
        }

        private void LoadInventory()
        {
            _inventoryBo = new InventoryBO();
            List<Inventory> items = _inventoryBo.GetAllActiveProducts();

            Inventory = new ObservableCollection<InventoryWrapper>();
            foreach (Inventory item in items)
            {
                InventoryWrapper wrapper = new InventoryWrapper(item);
                wrapper.CategoryName = item.Category.Name;
                wrapper.BrandName = item.Brand.Name;
                Inventory.Add(wrapper);
            }
        }

        private void ReloadInventory(InventoryChangedEventArgs args)
        {
            if(args.Action == EventAction.Add)
            {
                CategoryBO categoryBO = new CategoryBO();
                Category c = categoryBO.GetCategory(args.Inventory.CategoryId);
                args.Inventory.Category = c;

                BrandBO brandBo = new BrandBO();
                Brand b = brandBo.GetBrand(args.Inventory.BrandId);
                args.Inventory.Brand = b;

                InventoryWrapper wrapper = new InventoryWrapper(args.Inventory);
                wrapper.CategoryName = c.Name;
                wrapper.BrandName = b.Name;
                Inventory.Add(wrapper);
            }
            else if(args.Action == EventAction.Update)
            {
                var item = Inventory.Where(x => x.Id == args.Inventory.Id).FirstOrDefault();
                item.Quantity = args.Inventory.Quantity;
                item.RetailRate = args.Inventory.RetailRate;
                item.PurchaseRate = args.Inventory.PurchaseRate;
                item.FirstPurchaseDate = args.Inventory.FirstPurchaseDate;
            }
        }

        private void RemoveItem(Inventory obj)
        {
            var item = Inventory.Where(x => x.Id == obj.Id).FirstOrDefault();
            Inventory.Remove(item);

            InventoryChangedEventArgs args = new InventoryChangedEventArgs
            {
                Inventory = obj,
                Action = EventAction.Remove
            };
            _eventAggregator.GetEvent<InventoryChangedEvent>().Publish(args);
        }
    }
}
