using POS.Model;
using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Events;

namespace POSSystem.UI.Event
{
    public class ChangeUserPasswordEvent : PubSubEvent<User>
    {

    }

    public class UserPasswordChangedEvent : PubSubEvent<User>
    {

    }

    public class BillingInfoUpdateEvent: PubSubEvent<BillingInfoUpdateEventArgs>
    {

    }

    public class UpdateCustomerInfoEvent: PubSubEvent<CustomerWrapper>
    {

    }

    public class ShowCustomerPurchaseEvent: PubSubEvent<long>
    {

    }

}
