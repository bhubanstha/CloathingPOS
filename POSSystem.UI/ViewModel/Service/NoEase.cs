using POSSystem.UI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel.Service
{
    public class NoEase : IEasingFunction
    {
        public double Ease(double value)
        {
            return 1d;
        }
    }
}
