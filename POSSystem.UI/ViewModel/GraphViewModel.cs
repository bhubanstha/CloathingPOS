using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel.Service;

namespace POSSystem.UI.ViewModel
{
    public class GraphViewModel : ViewModelBase
    {
        private PlotModel _plotModel = null;
        private IColorService _colorService;
        public PlotModel PlotModel 
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                OnPropertyChanged();
            }
        }
        public List<Inventory> Products { get; set; }
        public List<Inventory> FilterProducts { get; set; }



        public GraphViewModel(IColorService colorService)
        {
            
            _colorService = colorService;
            GetAllProducts();
           
            
        }

        private void GetAllProducts()
        {
            InventoryBO inventoryBO = new InventoryBO();
            Products = inventoryBO.GetAllActiveProducts();
        }

        public void CreateGraphModel()
        {
            PlotModel = new PlotModel();
            var pnls = new List<Pnl>();
            var pnls1 = new List<Pnl>();
            var pnls2 = new List<Pnl>();


            pnls = MockPurchaseHistory();
            pnls1 = MockSalesHistory();
            pnls2 = MockStock(2021, ref pnls, ref pnls1);

            var max1 = pnls.Max(x => x.Value);
            var max2 = pnls1.Max(x => x.Value);
            var max3 = pnls2.Max(x => x.Value);

            var minimum = 0;
            var maximum = max1 > max2 && max1 > max3 ? max1 : (max2 > max1 && max2 > max3) ? max2 : max3;

            maximum += 10;

            var plotModel = this.PlotModel;
            //plotModel.Title = "Line Series Animation Demo";

            string hex = _colorService.GetColorHex(_colorService.GetColor("Yellow"));
            var series = new LineSeries
            {
                Title = "Purchase",
                ItemsSource = pnls,
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse(hex),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#FFFFFFFF"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Cross,
                StrokeThickness = 2,
            };


            hex = _colorService.GetColorHex(_colorService.GetColor("Red"));
            var series1 = new LineSeries
            {
                Title = "Sells",
                ItemsSource = pnls1,
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse(hex),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#FFFFFFFF"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Cross,
                StrokeThickness = 2,
            };

            hex = _colorService.GetColorHex(_colorService.GetColor("Purple"));
            var series2 = new LineSeries
            {
                Title = "Stock",
                ItemsSource = pnls2,
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse(hex),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#FFFFFFFF"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Cross,
                StrokeThickness = 2,
            };


            plotModel.Series.Add(series);
            plotModel.Series.Add(series1);
            plotModel.Series.Add(series2);

            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IntervalType = DateTimeIntervalType.Months,
                IntervalLength = 1,
                StringFormat = "MMM"
            };
            plotModel.Axes.Add(dateTimeAxis);

            var margin = (maximum - minimum) * 0.05;

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0, //minimum - margin,
                Maximum = maximum + margin,
            };
            plotModel.Axes.Add(valueAxis);
            plotModel.LegendTitle = "This is legend";
            plotModel.LegendPosition = LegendPosition.LeftTop;
            plotModel.InvalidatePlot(true);
        }

        private List<Pnl> MockPurchaseHistory()
        {
            List<Pnl> points = new List<Pnl>
            {
                new Pnl{Time =new DateTime(2021,1, 1),  Value = 10},
                new Pnl{Time =new DateTime(2021,1, 5),  Value = 16},
                new Pnl{Time =new DateTime(2021,1, 17),  Value = 70},
                //new Pnl{Time =new DateTime(2021,2, 1),  Value= 5},
                new Pnl{Time =new DateTime(2021,3, 1),  Value= 150},
                new Pnl{Time =new DateTime(2021,4, 1),  Value= 106},
                new Pnl{Time =new DateTime(2021,5, 1),  Value= 107},
                //new Pnl{Time =new DateTime(2021,6, 1),  Value= 130},
                //new Pnl{Time =new DateTime(2021,7, 1),  Value= 170},
                //new Pnl{Time =new DateTime(2021,8, 1),  Value= 120},
                //new Pnl{Time =new DateTime(2021,9, 1),  Value= 10},
                //new Pnl{Time =new DateTime(2021,10, 1), Value = 90},
                //new Pnl{Time =new DateTime(2021,11, 1), Value = 50},
                //new Pnl{Time =new DateTime(2021,12, 1), Value = 40}
            };
            return points;
        }

        private List<Pnl> MockSalesHistory()
        {
            List<Pnl> points = new List<Pnl>
            {
                new Pnl{Time =new DateTime(2021,1, 1),  Value = 3},
                new Pnl{Time =new DateTime(2021,2, 1),  Value = 0},
                new Pnl{Time =new DateTime(2021,3, 1),  Value = 15},
                new Pnl{Time =new DateTime(2021,4, 1),  Value = 100},
                new Pnl{Time =new DateTime(2021,5, 1),  Value = 80},
                //new Pnl{Time =new DateTime(2021,6, 1),  Value = 100},
                //new Pnl{Time =new DateTime(2021,7, 1),  Value = 130},
                //new Pnl{Time =new DateTime(2021,8, 1),  Value = 80},
                //new Pnl{Time =new DateTime(2021,9, 1),  Value = 0},
                //new Pnl{Time =new DateTime(2021,10, 1), Value = 50},
                //new Pnl{Time =new DateTime(2021,11, 1), Value = 10},
                //new Pnl{Time =new DateTime(2021,12, 1), Value = 5}
            };
            return points;
        }

        private List<Pnl> MockStock(int year, ref List<Pnl> purchaseHistory, ref List<Pnl> salesHistory)
        {
            List<Pnl> stock = new List<Pnl>();
            double totalPurchase = 0;
            double totalSales = 0;
            int maxMonth1 = purchaseHistory.Select(x => x.Time.Month).Max();
            int maxMonth2 = salesHistory.Select(x => x.Time.Month).Max();
            int maxMonth = maxMonth1 > maxMonth2 ? maxMonth1 : maxMonth2;

            for (int month = 1; month <= maxMonth; month++)
            {
                double purchase = purchaseHistory.Where(x => x.Time.Month == month).Sum(x => x.Value);
                double sales = salesHistory.Where(x => x.Time.Month == month).Sum(x => x.Value);
                totalPurchase += purchase;
                totalSales += sales;
                int maxDayInMonth = (month == DateTime.Now.Month && month == maxMonth) ? DateTime.Now.Day: DateTime.DaysInMonth(year, month);
                if(month==1)
                {
                    stock.Add(new Pnl
                    {
                        Time = new DateTime(year, month,1),
                        Value = LastYearStock(year)
                    });
                }
                stock.Add(new Pnl
                {
                    Time = new DateTime(year, month, maxDayInMonth),
                    Value = totalPurchase - totalSales
                });
            }

            return stock;
        }

        private int LastYearStock(int thisYear)
        {
            InventoryBO inventoryBO = new InventoryBO();
            var qty = inventoryBO.GetAllActiveProducts().Where(x => x.FirstPurchaseDate.Year == thisYear - 1).Sum(x => x.Quantity);
            return qty;
        }
    }

}
