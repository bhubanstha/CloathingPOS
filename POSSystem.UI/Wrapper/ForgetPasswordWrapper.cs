using POS.Model.ViewModel;
using POSSystem.UI.UIModel;

namespace POSSystem.UI.Wrapper
{
    public class ForgetPasswordWrapper : WrapperBase<ForgetPasswordModel>
    {
        private string _pwdMsg;
        private string _conPwdMsg;
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
            set { 
                SetValue(value);
                ClearErrors(nameof(ConfirmPassword));
                PasswordMessage = GetFirstError(nameof(Password));
                ConfirmPasswordMessage = GetFirstError(nameof(ConfirmPassword));

                OnPropertyChanged(nameof(PasswordMessage));
                OnPropertyChanged(nameof(ConfirmPasswordMessage));
            }
        }

        public string ConfirmPassword
        {
            get { return GetValue<string>(); }
            set 
            { 
                SetValue(value);
                ClearErrors(nameof(Password));
                ConfirmPasswordMessage = GetFirstError(nameof(ConfirmPassword));
                PasswordMessage = GetFirstError(nameof(Password));

                OnPropertyChanged(nameof(ConfirmPasswordMessage));
                OnPropertyChanged(nameof(PasswordMessage));
            }
        }

        public string PasswordMessage
        {
            get { return _pwdMsg; }
            set
            {
                _pwdMsg = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPasswordMessage
        {
            get { return _conPwdMsg; }
            set
            {
                _conPwdMsg = value;
                OnPropertyChanged();
            }
        }
    }
}
