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
                MarkerStroke = OxyColor.Parse(hex),
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
                MarkerStroke = OxyColor.Parse(hex),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Cross,
                StrokeThickness = 2,
            };


            plotModel.Series.Add(series);
            plotModel.Series.Add(series1);
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
    }
}
