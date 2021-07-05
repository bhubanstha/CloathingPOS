﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities.PDF;
using POSSystem.UI.Event;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Views.Dialog;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class InventoryListViewModel : ViewModelBase
    {
        private bool _isQrGenerating =false; 
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

        public bool IsQrGenerating
        {
            get { return _isQrGenerating; }
            set { _isQrGenerating = value; OnPropertyChanged(); }
        }



        public ICommand DeleteInventoryItemCommand { get; }
        public ICommand AddInventoryItemStockCommand { get; }
        public ICommand EditInventoryItemCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand PrintQrCodeCommand { get; }


        public InventoryListViewModel(UpdateInventoryDialog updateInventoryDialog, IEventAggregator eventAggregator)
        {
            _updateInventoryDialog = updateInventoryDialog;
            _eventAggregator = eventAggregator;
            _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            DeleteInventoryItemCommand = new DelegateCommand<InventoryWrapper>(DeleteInventoryItem);
            AddInventoryItemStockCommand = new DelegateCommand<InventoryWrapper>(OnAddInventoryItemStock);
            EditInventoryItemCommand = new DelegateCommand<InventoryWrapper>(OnInventoryItemEdit);
            LoadDataCommand = new DelegateCommand(OnLoadDataExecute);
            PrintQrCodeCommand = new DelegateCommand(OnPrintQrCodeExecute);
            eventAggregator.GetEvent<InventoryChangedEvent>().Subscribe(ReloadInventory);
            eventAggregator.GetEvent<BranchSwitchedEvent>().Subscribe(ReloadBranchData);
        }

        private async void OnPrintQrCodeExecute()
        {
            IsQrGenerating = true;
            List<Inventory> selectedItems = Inventory.Where(x => x.IsSelected == true).Select(x => x.Model).ToList();
            if(selectedItems.Count >0 )
            {
                CreateQRCode createQRCode = new CreateQRCode(StaticContainer.Shop.PdfPassword);
                string pdfPath = await createQRCode.CreateLabel(selectedItems);
                PDFViewerWindow window = new PDFViewerWindow(pdfPath, StaticContainer.Shop.PdfPassword, selectedItems);
                IsQrGenerating = false;
                window.ShowDialog();                
            }
            else
            {
                IsQrGenerating = false;
                StaticContainer.ShowNotification("No Item Selected", "You haven't selected any item to print QR Code. Please selecte at least 1 item.", NotificationType.Information);
            }
        }

        private void OnLoadDataExecute()
        {
            LoadInventory();
        }

        private void ReloadBranchData(BranchWrapper obj)
        {
            Inventory = null;
        }

        private void OnInventoryItemEdit(InventoryWrapper obj)
        {
            InventoryChangedEventArgs args = new InventoryChangedEventArgs
            {
                Inventory = obj.Model,
                Action = EventAction.Edit
            };
            _eventAggregator.GetEvent<InventoryChangedEvent>().Publish(args);
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
            List<Inventory> items = _inventoryBo.GetAllActiveProducts(StaticContainer.ActiveBranchId);

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
