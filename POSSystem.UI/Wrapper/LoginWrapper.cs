using iText.Layout.Properties;
using POS.Model.ViewModel;
using POSSystem.UI.UIModel;
using System.Collections.Generic;

namespace POSSystem.UI.Wrapper
{
    public class LoginWrapper : WrapperBase<LoginModel>
    {
        private string _pwdErrorMsg;
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
            set 
            { 
                SetValue(value);
                _pwdErrorMsg = GetFirstError(nameof(Password));
                OnPropertyChanged(nameof(PasswordErrorMessage));
            }
        }

        public bool RememberMe
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string PasswordErrorMessage
        {
            get { return _pwdErrorMsg; }
            private set 
            { 
                OnPropertyChanged(); 
            }
        }


        //protected override IEnumerable<string> ValidateProperty(string propertyName)
        //{
        //    List<string> errors = new List<string>();
        //    if (propertyName == "UserName")
        //    {
        //        errors.Add("User name already exists");
        //    }
        //    return errors;
        //}

    }
}
