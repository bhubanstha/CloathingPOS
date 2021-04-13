﻿using POSSystem.UI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel.Service
{
    public class CircleEase : IEasingFunction
    {
        public double Ease(double value)
        {
            value = Math.Max(0.0, Math.Min(1.0, value));

            var num1 = 1.0;
            var num2 = 1.0;
            var num3 = value;
            var num4 = num3 * num3;
            var num5 = Math.Sqrt(num2 - num4);

            return num1 - num5;
        }
    }
}
