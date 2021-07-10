using iText.Layout.Properties;
using POS.Model.ViewModel;
using POSSystem.UI.UIModel;
using System;
using System.Collections.Generic;

namespace POSSystem.UI.Wrapper
{
    public class LoginWrapper : WrapperBase<LoginModel>
    {
        private string _pwdErrorMsg;
        private string _branchErrorMsg;
        public LoginWrapper(LoginModel obj) : base(obj)
        {
        }

        public Int64 BranchId
        {
            get { return GetValue<Int64>(); }
            set { 
                SetValue(value);
                _branchErrorMsg = GetFirstError(nameof(BranchId));
                OnPropertyChanged(nameof(BranchErrorMessage));
            }
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
                _pwdErrorMsg = value;
                OnPropertyChanged(); 
            }
        }

        public string BranchErrorMessage
        {
            get { return _branchErrorMsg; }
            private set
            {
                _branchErrorMsg = value;
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
