using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Interface
{
    public interface IAnimatablePoint
    {
        double FinalX { get; set; }

        double FinalY { get; set; }

        double X { get; set; }

        double Y { get; set; }
    }
}
