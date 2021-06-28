using POS.Core.Model;
using Prism.Events;

namespace POSSystem.WPF.UI.Event
{
    public class ChangeUserPasswordEvent : PubSubEvent<User>
    {

    }

    public class UserPasswordChangedEvent : PubSubEvent<User>
    {

    }

}
