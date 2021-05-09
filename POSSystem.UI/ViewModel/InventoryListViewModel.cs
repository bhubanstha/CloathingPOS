using MahApps.Metro.Controls;
using POS.BusinessRule;
using POS.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class InventoryListViewModel : ViewModelBase
    {
        private InventoryBO _inventoryBo;
        private MetroWindow _window;

        private ObservableCollection<Inventory> _inventory;

        public ObservableCollection<Inventory> Inventory
        {
            get { return _inventory; }
            set { _inventory = value; OnPropertyChanged(); }
        }


        public ICommand DeleteInventoryItemCommand { get; }

        public InventoryListViewModel()
        {
            _window = Application.Current.MainWindow as MetroWindow;
            DeleteInventoryItemCommand = new DelegateCommand<Inventory>(DeleteInventoryItem);
            LoadInventory();
        }

        private void DeleteInventoryItem(Inventory obj)
        {
            _inventoryBo = new InventoryBO();
            _inventoryBo.RemoveItem(obj);
            LoadInventory();
        }

        private void LoadInventory()
        {
            _inventoryBo = new InventoryBO();
            List<Inventory> items = _inventoryBo.GetAllActiveProducts();
            
            Inventory = new ObservableCollection<Inventory>();
            //for (int i = 0; i < 50; i++)
            //{
                foreach (Inventory item in items)
                {
                    Inventory.Add(item);
                }
            //}
        }
    }
}
