using POS.Core.Model;
using POS.Core.Model.ViewModel;
using System;
using System.ComponentModel;

namespace POSSystem.WPF.UI.Wrapper
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
