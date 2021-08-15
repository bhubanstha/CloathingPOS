using MahApps.Metro.Controls;
using MoonPdfLib;
using POS.BusinessRule;
using POS.Model.ViewModel;
using POS.Utilities;
using POS.Utilities.PDF;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class ReportViewModel : NotifyPropertyChanged
    {
        private string reportPdfPath="";
        private int _year;
        private int _month;
        private decimal _totalProfit;
        private bool _isReportGenerating = false;
        private bool _isPdfOptionsVisible = false;
        private ObservableCollection<ProfitLossReport> _report;

        public MoonPdfPanel PdfViewer { get; set; }

        public int Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged(); }
        }

        public int Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged(); }
        }

        public decimal TotalProfit
        {
            get { return _totalProfit; }
            set { _totalProfit = value;  OnPropertyChanged(); }
        }


        public ObservableCollection<Month> Months { get; set; }
        public ObservableCollection<Year> Years{ get; set; }
        public ObservableCollection<ProfitLossReport> Report
        {
            get { return _report; }
            set { _report = value; OnPropertyChanged(); }
        }

       

        public bool IsReportGenerating
        {
            get { return _isReportGenerating; }
            set { _isReportGenerating = value; OnPropertyChanged(); }
        }

        

        public bool IsPdfOptionsVisible 
        {
            get { return _isPdfOptionsVisible; }
            set { _isPdfOptionsVisible = value; OnPropertyChanged(); }
        }


        public ICommand LoadReportCommand { get; set; }
        public ICommand PrintReportCommand { get; set; }
        public ICommand OpenReportCommand { get; set; }


        public ReportViewModel()
        {
            Year = StaticContainer.AppDeployedYear;
            Month = 1;
            Months = new ObservableCollection<Month>(MonthProvider.GetMonths());
            Years = new ObservableCollection<Year>(YearProvider.GetYears(Year));
            LoadReportCommand = new DelegateCommand(OnLoadReportExecute);
            PrintReportCommand = new DelegateCommand(OnPrintReportCommand);
            OpenReportCommand = new DelegateCommand(OnOpenReportCommand);
        }

        private void OnOpenReportCommand()
        {
            PDFViewerWindow window = new PDFViewerWindow(reportPdfPath, StaticContainer.Shop.PdfPassword);
            window.ShowDialog();
        }

        private void OnPrintReportCommand()
        {
            PdfViewer.Print();
        }

        private async void OnLoadReportExecute()
        {
            SalesBO bo = new SalesBO();
            IsReportGenerating = true;
            IsPdfOptionsVisible = false;
            List<ProfitLossReport> rpt = await bo.GetProfitLoss(Year, Month);
            if (rpt.Count > 0)
            {
                TotalProfit = rpt.Sum(x => x.Profit);
                CreateProfitLossReport createProfitLossReport = new CreateProfitLossReport();
                reportPdfPath  = await createProfitLossReport.CreateReport(rpt, StaticContainer.Shop, TotalProfit, Year, Month);
                PdfViewer.OpenFile(reportPdfPath, StaticContainer.Shop.PdfPassword);
                IsPdfOptionsVisible = true;
                //Report = new ObservableCollection<ProfitLossReport>(rpt);
            }
            else
            {
                Flyout f = StaticContainer.NoSearchResultFlyout;
                f.IsOpen = !f.IsOpen;
                if (PdfViewer.IsLoaded)
                {
                    PdfViewer.Unload();
                }
            }
            IsReportGenerating = false;
        }
    }
}
