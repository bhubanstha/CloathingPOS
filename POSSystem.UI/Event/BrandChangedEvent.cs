using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Events;

namespace POSSystem.UI.Event
{
    public class BrandChangedEvent : PubSubEvent<BrandChangedEventArgs>
    {

    }

    public class BranchChangedEvent : PubSubEvent<BranchChangedEventArgs>
    {

    }

    public class BranchSwitchedEvent: PubSubEvent<BranchWrapper>
    {

    }

}
