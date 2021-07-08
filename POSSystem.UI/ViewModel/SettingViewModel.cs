using log4net;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class SettingViewModel : NotifyPropertyChanged
    {
        private bool _calVAT;
        private bool _printInvoice;
        private string _pdfPassword;
        private ILog _log;
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
        public SettingViewModel(ILogger logger)
        {
            CalculateVAT = StaticContainer.Shop.CalculateVATOnSales;
            PrintInvoice = StaticContainer.Shop.PrintInvoice;
            PdfPassword = StaticContainer.Shop.PdfPassword;
            _log = logger.GetLogger(typeof(SettingViewModel));
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
                    PANNumber = StaticContainer.Shop.PANNumber,
                    LogoPath = StaticContainer.Shop.LogoPath,
                    CalculateVATOnSales = CalculateVAT,
                    PrintInvoice = PrintInvoice,
                    PdfPassword = PdfPassword
                };
                ShopBO bo = new ShopBO();
                await bo.UpdateShop(s);
                StaticContainer.Shop.CalculateVATOnSales = s.CalculateVATOnSales;
                StaticContainer.Shop.PrintInvoice = s.PrintInvoice;
                StaticContainer.Shop.PdfPassword = s.PdfPassword;
            }
            catch (Exception ex)
            {
                _log.Error("SettingViewModel.OnSettingSave", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Success);
            }
        }
    }
}
