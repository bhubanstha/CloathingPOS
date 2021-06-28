using Notifications.Wpf.Core;
using POS.Core.BusinessRule;
using POS.Core.Model;
using POSSystem.WPF.UI.Service;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace POSSystem.WPF.UI.ViewModel
{
    public class SettingViewModel : NotifyPropertyChanged
    {
        private bool _calVAT;
        private bool _printInvoice;
        private string _pdfPassword;

        public bool CalculateVAT
        {
            get { return _calVAT; }
            set { _calVAT = value; OnPropertyChanged(); }
        }


        public bool PrintInvoice
        {
            get { return _printInvoice; }
            set { _printInvoice = value; OnPropertyChanged(); }
        }

       

        public string PdfPassword
        {
            get { return _pdfPassword; }
            set { _pdfPassword = value; OnPropertyChanged(); }
        }

        public ICommand SaveSettingCommand { get; }
        public SettingViewModel()
        {
            CalculateVAT = StaticContainer.Shop.CalculateVATOnSales;
            PrintInvoice = StaticContainer.Shop.PrintInvoice;
            PdfPassword = StaticContainer.Shop.PdfPassword;

            SaveSettingCommand = new DelegateCommand(OnSettingSave);
        }

        private async void OnSettingSave()
        {
            try
            {
                Shop s = new Shop
                {
                    Id = StaticContainer.Shop.Id,
                    Name = StaticContainer.Shop.Name,
                    Address = StaticContainer.Shop.Address,
                    PANNumber = StaticContainer.Shop.PANNumber,
                    LogoPath = StaticContainer.Shop.LogoPath,
                    CalculateVATOnSales = CalculateVAT,
                    PrintInvoice = PrintInvoice,
                    PdfPassword = PdfPassword
                };
                ShopBO bo = new ShopBO();
                await bo.UpdateShop(s);
                StaticContainer.Shop = s;
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Success);
            }
        }
    }
}
