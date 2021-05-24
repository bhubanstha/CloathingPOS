using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel.Extensions;
using POSSystem.UI.ViewModel.Service;

namespace POSSystem.UI.ViewModel
{
    public class LineSeriesViewModel : AnimationViewModelBase
    {

        private IColorService _colorService;

        public LineSeriesViewModel(IColorService colorService)
        {
            _colorService = colorService;
            var pnls = new List<Pnl>();
            var pnls1 = new List<Pnl>();
            var pnls2 = new List<Pnl>();

            
            pnls = MockPurchaseHistory();
            pnls1 = MockSalesHistory();
            pnls2 = MockStock(2021, ref pnls, ref pnls1);
            var min1 = pnls.Min(x => x.Value);
            var min2 = pnls1.Min(x => x.Value);
            var max1 = pnls.Max(x => x.Value);
            var max2 = pnls1.Max(x => x.Value);

            var minimum = min1<min2? min1 : min2;
            var maximum = max1>max2? max1: max2;
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
            var annotation = new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = 0
            };
            //plotModel.Annotations.Add(annotation);

            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IntervalType = DateTimeIntervalType.Months,
                IntervalLength = 1,
                StringFormat="MMM"
            };
            plotModel.Axes.Add(dateTimeAxis);

            var margin = (maximum - minimum) * 0.05;

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = minimum - margin,
                Maximum = maximum + margin,
            };
            plotModel.Axes.Add(valueAxis);
        }

        public override bool SupportsEasingFunction { get { return true; } }

        public override async Task AnimateAsync(AnimationSettings animationSettings)
        {
            var plotModel = this.PlotModel;
            foreach (var s in plotModel.Series)
            {

                if (s is LineSeries)
                {
                    await plotModel.AnimateSeriesAsync((LineSeries)s, animationSettings);
                }
            }

            //var series = plotModel.Series.First() as LineSeries;
            //if (series != null)
            //{
            //    await plotModel.AnimateSeriesAsync(series, animationSettings);
            //}
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
                new Pnl{Time =new DateTime(2021,6, 1),  Value= 130},
                new Pnl{Time =new DateTime(2021,7, 1),  Value= 170},
                new Pnl{Time =new DateTime(2021,8, 1),  Value= 120},
                new Pnl{Time =new DateTime(2021,9, 1),  Value= 10},
                new Pnl{Time =new DateTime(2021,10, 1), Value = 90},
                new Pnl{Time =new DateTime(2021,11, 1), Value = 50},
                new Pnl{Time =new DateTime(2021,12, 1), Value = 40}
            };
            return points;
        }

        private List<Pnl> MockSalesHistory()
        {
            List<Pnl> points = new List<Pnl>
            {
                new Pnl{Time =new DateTime(2021,1, 1),  Value = 3},
                new Pnl{Time =new DateTime(2021,2, 1),  Value = 2},
                new Pnl{Time =new DateTime(2021,3, 1),  Value = 15},
                new Pnl{Time =new DateTime(2021,4, 1),  Value = 100},
                new Pnl{Time =new DateTime(2021,5, 1),  Value = 80},
                new Pnl{Time =new DateTime(2021,6, 1),  Value = 100},
                new Pnl{Time =new DateTime(2021,7, 1),  Value = 130},
                //new Pnl{Time =new DateTime(2021,8, 1),  Value = 80},
                new Pnl{Time =new DateTime(2021,9, 1),  Value = 0},
                new Pnl{Time =new DateTime(2021,10, 1), Value = 50},
                new Pnl{Time =new DateTime(2021,11, 1), Value = 10},
                new Pnl{Time =new DateTime(2021,12, 1), Value = 5}
            };
            return points;
        }

        private List<Pnl> MockStock(int year, ref List<Pnl> purchaseHistory, ref List<Pnl> salesHistory)
        {
            List<Pnl> stock = new List<Pnl>();
            for (int month = 1; month < 13; month++)
            {
                double purchase = purchaseHistory.Where(x => x.Time.Month == month).Sum(x => x.Value);
                double sales = salesHistory.Where(x => x.Time.Month == month).Sum(x => x.Value);
                int maxDayInMonth = DateTime.DaysInMonth(year, month);
                stock.Add(new Pnl
                {
                    Time = new DateTime(year, month, maxDayInMonth),
                    Value = purchase-sales
                });
            }

            return stock;
        }
    }

}
