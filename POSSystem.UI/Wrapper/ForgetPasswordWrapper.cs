using POS.Model.ViewModel;
using POSSystem.UI.UIModel;

namespace POSSystem.UI.Wrapper
{
    public class ForgetPasswordWrapper : WrapperBase<ForgetPasswordModel>
    {
        public ForgetPasswordWrapper(ForgetPasswordModel model) :base(model)
        {

        }


        public string UserName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Password
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string ConfirmPassword
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
