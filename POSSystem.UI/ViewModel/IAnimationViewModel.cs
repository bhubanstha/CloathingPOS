using POSSystem.UI.ViewModel.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel
{
    public interface IAnimationViewModel
    {
        bool SupportsEasingFunction { get; }

        Task AnimateAsync();
        Task AnimateAsync(AnimationSettings animationSettings);
    }
}
