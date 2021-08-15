using POS.BusinessRule;
using POS.Model.ViewModel;
using POS.Utilities;
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

        private int _year;
        private int _month;
        private decimal _totalProfit;
        private ObservableCollection<ProfitLossReport> _report;

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

        public ICommand LoadReportCommand { get; set; }


        public ReportViewModel()
        {
            Year = StaticContainer.AppDeployedYear;
            Month = 1;
            Months = new ObservableCollection<Month>(MonthProvider.GetMonths());
            Years = new ObservableCollection<Year>(YearProvider.GetYears(Year));
            LoadReportCommand = new DelegateCommand(OnLoadReportExecute);
        }

        private async void OnLoadReportExecute()
        {
            SalesBO bo = new SalesBO();
            List<ProfitLossReport> rpt = await bo.GetProfitLoss(Year, Month);
            Report = new ObservableCollection<ProfitLossReport>(rpt);
            TotalProfit = rpt.Sum(x => x.Profit);
        }
    }
}
