using POSSystem.UI.Interface;
using POSSystem.UI.ViewModel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel.Service
{
    public class AnimationSettings
    {
        public AnimationSettings()
        {
            this.HorizontalPercentage = 70;
            this.VerticalPercentage = 30;

            this.Delay = TimeSpan.FromMilliseconds(AnimationExtensions.DefaultAnimationDelay);
            this.Duration = TimeSpan.FromMilliseconds(AnimationExtensions.DefaultAnimationDuration);
            this.FrameDuration = TimeSpan.FromMilliseconds(AnimationExtensions.DefaultAnimationFrameDuration);
        }

        public double? MinimumValue { get; set; }

        public double HorizontalPercentage { get; set; }

        public double VerticalPercentage { get; set; }

        public TimeSpan Delay { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan FrameDuration { get; set; }

        public IEasingFunction EasingFunction { get; set; }
    }
}
