using POS.Model;
using Prism.Events;

namespace POSSystem.UI.Event
{
    public class ChangeUserPasswordEvent : PubSubEvent<User>
    {

    }

    public class UserPasswordChangedEvent : PubSubEvent<User>
    {

    }

}
