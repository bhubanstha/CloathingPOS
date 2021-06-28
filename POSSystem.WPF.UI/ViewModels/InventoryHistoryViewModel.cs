﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Input;

namespace POSSystem.WPF.UI.ViewModel
{
    public class InventoryHistoryViewModel
    {

        private InventoryHistoryWrapper _inventory;
        private IEventAggregator _eventAggregator;

        public InventoryHistoryWrapper Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }


        public ICommand ClosePopupCommand { get; private set; }
        public ICommand UpdateInventoryCommand { get; private set; }
        public InventoryHistoryViewModel(IEventAggregator eventAggregator)
        {

            Inventory = new InventoryHistoryWrapper(new InventoryHistory())
            {
                PurchaseDate = DateTime.Now
            };
            _eventAggregator = eventAggregator;
            // _dialog = dialog;

            eventAggregator.GetEvent<InventoryUpdatePopupOpenEvent>().Subscribe(OnPopuOpen);
            ClosePopupCommand = new DelegateCommand(OnClosePopup);
            UpdateInventoryCommand = new DelegateCommand(OnInventoryUpdate);

        }

        private async void OnInventoryUpdate()
        {
            try
            {
                
                Inventory.Model.Inventory.Quantity += Inventory.Quantity;
                Inventory.Model.Inventory.RetailRate = Inventory.RetailRate;
                Inventory.Model.Inventory.PurchaseRate = Inventory.PurchaseRate;
                Inventory.Model.Inventory.FirstPurchaseDate = Inventory.PurchaseDate;
                InventoryChangedEventArgs args = new InventoryChangedEventArgs
                {
                    Inventory = Inventory.Model.Inventory,
                    Action = EventAction.Update
                };

                InventoryBO inventoryBO = new InventoryBO();
                await inventoryBO.UpdateInventory(Inventory.Model.Inventory, Inventory.Model);
                _eventAggregator.GetEvent<InventoryChangedEvent>().Publish(args);
                StaticContainer.ShowNotification("Updated", "Inventory restocked successfully.", NotificationType.Success);
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
            finally
            {
                OnClosePopup();
            }
        }

        private void OnPopuOpen(Inventory obj)
        {
            Inventory.PurchaseDate = DateTime.Now;
            Inventory.Quantity = 1;
            Inventory.PurchaseRate = obj.PurchaseRate;
            Inventory.RetailRate = obj.RetailRate;
            Inventory.InventoryId = obj.Id;
            Inventory.Model.Inventory = obj;
            Inventory.Heading = $"{obj.Category.Name}-{obj.Name}";
        }

        private void OnClosePopup()
        {
            MetroWindow window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            window.HideMetroDialogAsync(StaticContainer.InventoryUpdateDialog);
        }
    }
}
