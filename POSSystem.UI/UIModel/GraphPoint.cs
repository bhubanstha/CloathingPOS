using OxyPlot.Axes;
using System;
using System.Diagnostics;

namespace POSSystem.UI.UIModel
{
    [DebuggerDisplay("X: {X} => {FinalX} | Y: {Y} => {FinalY}")]
    public class GraphPoint
    {
        public double FinalX { get; set; }
        public double FinalY { get; set; }
        public double Date { get; set; }
        public double Quantity { get; set; }

        public DateTime Time
        {
            get { return DateTimeAxis.ToDateTime(this.Date); }
            set
            {
                var finalX = DateTimeAxis.ToDouble(value);

                this.FinalX = finalX;
                this.Date = finalX;
            }
        }

        public double Value
        {
            get { return this.Quantity; }
            set
            {
                this.FinalY = value;
                this.Quantity = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0:yyyy/mm/dd} {1:0.0}", this.Time, this.Value);
        }
    }
}
