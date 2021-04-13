using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel.Service
{
    public class AnimationFrame
    {
        private static readonly TimeSpan DefaultDuration = TimeSpan.FromMilliseconds(15);

        public AnimationFrame()
        {
            AnimationPoints = new List<AnimationPoint>();
            Duration = DefaultDuration;
        }

        public TimeSpan Duration { get; set; }

        public List<AnimationPoint> AnimationPoints { get; private set; }
    }
}
