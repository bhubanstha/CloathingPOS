using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace POSSystem.UI.ViewModel.Service
{
    [DebuggerDisplay("{X} / {Y} (IsVisible = {IsVisible})")]
    public class AnimationPoint
    {
        public AnimationPoint()
        {
            this.IsVisible = true;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public bool IsVisible { get; set; }
    }
}
