using MahApps.Metro.Controls;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class InventoryListViewModel : ViewModelBase
    {
        private InventoryBO _inventoryBo;
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

        public InventoryListViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DeleteInventoryItemCommand = new DelegateCommand<InventoryWrapper>(DeleteInventoryItem);
            eventAggregator.GetEvent<InventoryChangedEvent>().Subscribe(ReloadInventory);
            LoadInventory();
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

                InventoryWrapper wrapper = new InventoryWrapper(args.Inventory);
                wrapper.CategoryName = c.Name;
                Inventory.Add(wrapper);
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
