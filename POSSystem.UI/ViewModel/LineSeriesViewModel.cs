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

            var random = new Random(50);
            var dateTime = DateTime.Today.Add(TimeSpan.FromHours(9));
            for (var pointIndex = 0; pointIndex < 50; pointIndex++)
            {
                pnls.Add(new Pnl
                {
                    Time = dateTime,
                    Value = -200 + random.Next(1000),
                });

                pnls1.Add(new Pnl
                {
                    Time = dateTime,
                    Value = -200 + random.Next(1000),
                });

                pnls2.Add(new Pnl
                {
                    Time = dateTime,
                    Value = -200 + random.Next(1000),
                });

                dateTime = dateTime.AddMinutes(1);
            }

            var minimum = pnls.Min(x => x.Value);
            var maximum = pnls.Max(x => x.Value);

            var plotModel = this.PlotModel;
            plotModel.Title = "Line Series Animation Demo";

            string hex = _colorService.GetColorHex(_colorService.GetColor("Yellow"));
            var series = new LineSeries
            {
                Title = "P & L",
                ItemsSource = pnls,
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse(hex),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse(hex),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Cross,
                StrokeThickness = 1,
            };

            var series1 = new AreaSeries
            {
                Title = "P & L",
                ItemsSource = pnls1,
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse("#1d2bc2"),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#1d2bc2"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Cross,
                StrokeThickness = 1,
            };

            var series2 = new LinearBarSeries
            {
                Title = "P & L",
                ItemsSource = pnls2,
                DataFieldX = "Time",
                DataFieldY = "Value",
                FillColor = OxyColor.Parse("#a13120"),
                StrokeColor = OxyColor.Parse("#e3ba34"),
                StrokeThickness = 1,
                BarWidth = 5
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
                IntervalType = DateTimeIntervalType.Hours,
                IntervalLength = 50
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
            var series = plotModel.Series.First() as LineSeries;
            if (series != null)
            {
                await plotModel.AnimateSeriesAsync(series, animationSettings);
            }
        }
    }
}
