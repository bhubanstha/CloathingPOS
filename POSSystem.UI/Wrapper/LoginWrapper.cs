using POSSystem.UI.UIModel;

namespace POSSystem.UI.Wrapper
{
    public class LoginWrapper : WrapperBase<LoginModel>
    {
        public LoginWrapper(LoginModel obj) : base(obj)
        {
        }


        public string UserName 
        { 
            get { return GetValue<string>(); }
            set { SetValue(value);}
        }

        public string Password
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool RememberMe
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }


    }
}
