using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel
{
    public class GraphViewModel : ViewModelBase
    {
        private PlotModel _plotModel = null;
        private Inventory _selectedItem = null;
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


        public Inventory SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();

            }
        }



        public GraphViewModel(IColorService colorService)
        {

            _colorService = colorService;
            GetAllProducts();


        }

        
        public async void CreateGraphModel()
        {
            PlotModel = new PlotModel();
            var purchasePoints = new List<GraphPoint>();
            var salePoints = new List<GraphPoint>();
            var stockPoints = new List<GraphPoint>();


            purchasePoints = PurchaseHistory();
            salePoints = SalesHistory();
            stockPoints = await Stock(2021,  purchasePoints,  salePoints);

            var max1 = purchasePoints.Count == 0 ? 0 : purchasePoints.Max(x => x.Value);
            var max2 = salePoints.Count == 0 ? 0 : salePoints.Max(x => x.Value);
            var max3 = stockPoints.Count == 0 ? 0 : stockPoints.Max(x => x.Value);

            var minimum = 0;
            var maximum = max1 > max2 && max1 > max3 ? max1 : (max2 > max1 && max2 > max3) ? max2 : max3;

            maximum += 5;



            PlotModel.Series.Add(CreateSeries(purchasePoints, "Purchase", "Yellow", MarkerType.Plus));
            PlotModel.Series.Add(CreateSeries(salePoints, "Sells", "Red", MarkerType.Star));
            PlotModel.Series.Add(CreateSeries(stockPoints, "Stock", "Purple", MarkerType.Circle));

            var margin = (maximum - minimum) * 0.05;

            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IntervalType = DateTimeIntervalType.Months,
                IntervalLength = 1,
                StringFormat = "MMM",
                IsZoomEnabled = false,
                MajorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.Parse("#dedad9"),
                MajorGridlineThickness = 2,
                Title = "Time"
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0, //minimum - margin,
                Maximum = maximum + margin,
                IsZoomEnabled = false,
                MajorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.Parse("#dedad9"),
                MajorGridlineThickness = 2,
                Title = "Quantity"
            };

            PlotModel.Axes.Add(dateTimeAxis);
            PlotModel.Axes.Add(valueAxis);
            PlotModel.LegendTitle = $"{SelectedItem.Name}-[{SelectedItem.Size}] History Graph";
            PlotModel.LegendPosition = LegendPosition.LeftTop;
            PlotModel.InvalidatePlot(true);
        }

        #region Private Methods
        private List<GraphPoint> PurchaseHistory()
        {

            InventoryHistoryBO bO = new InventoryHistoryBO();
            List<InventoryHistory> result = bO.GetHistory(SelectedItem.Id).Result;

            List<GraphPoint> points = new List<GraphPoint>();
            foreach (InventoryHistory history in result)
            {
                points.Add(new GraphPoint
                {
                    Time = history.PurchaseDate.Date,
                    Value = history.Quantity
                });
            }
            return points;
        }

        private List<GraphPoint> SalesHistory()
        {

            SalesBO bO = new SalesBO();
            List<Sales> results = bO.GetSalesHistory(SelectedItem.Id).Result;
            List<GraphPoint> points = new List<GraphPoint>();
            foreach (Sales sale in results)
            {
                points.Add(new GraphPoint
                {
                    Time = sale.Bill.BillDate.Date,
                    Value = sale.SalesQuantity
                });
            }
            return points;
        }

        private async Task<List<GraphPoint>> Stock(int year,  List<GraphPoint> purchaseHistory,  List<GraphPoint> salesHistory)
        {
            List<GraphPoint> stock = new List<GraphPoint>();

            if (purchaseHistory.Count > 0 || salesHistory.Count > 0)
            {
                double totalPurchase = 0;
                double totalSales = 0;
                int maxMonth1 = purchaseHistory.Count == 0 ? 0 : purchaseHistory.Select(x => x.Time.Month).Max();
                int maxMonth2 = salesHistory.Count == 0 ? maxMonth1 : salesHistory.Select(x => x.Time.Month).Max();
                int maxMonth = maxMonth1 > maxMonth2 ? maxMonth1 : maxMonth2;

                int minMonth1 = purchaseHistory.Count == 0 ? 0 : purchaseHistory.Select(x => x.Time.Month).Min();
                int minMonth2 = salesHistory.Count == 0 ? minMonth1 : salesHistory.Select(x => x.Time.Month).Min();
                int minMonth = minMonth1 < minMonth2 ? minMonth1 : minMonth2;

                minMonth = minMonth == 0 && maxMonth == 0 ? 0 : minMonth == 0 ? maxMonth : minMonth;



                for (int month = minMonth; month <= maxMonth; month++)
                {
                    double purchase = purchaseHistory.Where(x => x.Time.Month == month).Sum(x => x.Value);
                    double sales = salesHistory.Where(x => x.Time.Month == month).Sum(x => x.Value);
                    totalPurchase += purchase;
                    totalSales += sales;
                    int maxDayInMonth = (month == DateTime.Now.Month && month == maxMonth) ? DateTime.Now.Day : DateTime.DaysInMonth(year, month);
                    if (month == 1)
                    {
                        stock.Add(new GraphPoint
                        {
                            Time = new DateTime(year, month, 1),
                            Value = await LastYearStock(year)
                        });
                    }
                    var stockQty = totalPurchase - totalSales;
                    stock.Add(new GraphPoint
                    {
                        Time = new DateTime(year, month, maxDayInMonth),
                        Value = stockQty < 0 ? 0 : stockQty
                    }); ;
                }
            }

            return stock;
        }

        private async Task<int> LastYearStock(int thisYear)
        {
            InventoryBO inventoryBO = new InventoryBO();
            List<Inventory> inventories = await inventoryBO.GetAllActiveProducts(StaticContainer.ActiveBranchId, "");
            var qty = inventories.Where(x => x.FirstPurchaseDate.Year == thisYear - 1).Sum(x => x.Quantity);
            return qty;
        }

        private async void GetAllProducts()
        {
            InventoryBO inventoryBO = new InventoryBO();
            Products = await inventoryBO.GetAllActiveProducts(StaticContainer.ActiveBranchId, "");
        }



        private LineSeries CreateSeries(List<GraphPoint> dataSource, string Title, string color, MarkerType markerType)
        {
            string hex = _colorService.GetColorHex(_colorService.GetColor(color));
            LineSeries series = new LineSeries
            {
                Title = Title,
                ItemsSource = dataSource,
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse(hex),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#FFFFFFFF"),
                MarkerStrokeThickness = 1.5,
                MarkerType = markerType,
                StrokeThickness = 2,
            };

            return series;
        } 
        #endregion
    }

}
