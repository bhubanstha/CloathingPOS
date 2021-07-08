using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Views.Dialog;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class BillingViewModel
    {
        private IEventAggregator _eventAggregator;
        private ILog _log;
        public BillEntityWrapper Bill { get; set; }

        public UpdateBillingInfoDialog Dialog { get; set; }
        public ICommand ClosePopupCommand { get; private set; }
        public ICommand UpdateBillingCommand { get; private set; }

        public BillingViewModel(IEventAggregator eventAggregator, ILogger logger)
        {
            this._eventAggregator = eventAggregator;
            this._log = logger.GetLogger(typeof(BillingViewModel));

            Bill = new BillEntityWrapper(new Bill());
            
            ClosePopupCommand = new DelegateCommand(OnClosePopup);
            UpdateBillingCommand = new DelegateCommand(OnBillingUpdate);

            eventAggregator.GetEvent<BillingInfoUpdateEvent>().Subscribe(OnDialogOpen);
        }

        private void OnDialogOpen(BillingInfoUpdateEventArgs arg)
        {
            if (arg.Action == EventAction.Edit)
            {
                BillBO bO = new BillBO();
                Bill b = bO.GetById(arg.BillId);
                Bill.BillDate = b.BillDate;
                Bill.BillingAddress = b.BillingAddress;
                Bill.BillingPAN = b.BillingPAN;
                Bill.Id = b.Id;
                Bill.BillTo = b.BillTo;
                Bill.VAT = b.VAT;
            }
        }

        private async void OnBillingUpdate()
        {
            try
            {
                BillBO bO = new BillBO();
                int i = await bO.Update(Bill.Model);
                if (i > 0)
                {
                    StaticContainer.ShowNotification("Updated", "Billing information updated.", NotificationType.Success);
                }
            }
            catch (Exception ex)
            {
                _log.Error("OnBillingUpdate", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Success);
            }
            finally
            {
                OnClosePopup();
                BillingInfoUpdateEventArgs arg = new BillingInfoUpdateEventArgs
                {
                    BillId = Bill.Id,
                    Action = EventAction.Update
                };
                _eventAggregator.GetEvent<BillingInfoUpdateEvent>().Publish(arg);
            }
        }

        private void OnClosePopup()
        {
            MetroWindow window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            window.HideMetroDialogAsync(Dialog);
        }
    }
}
