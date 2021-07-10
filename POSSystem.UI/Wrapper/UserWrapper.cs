using POS.Model;
using POSSystem.UI.Enum;
using POSSystem.UI.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace POSSystem.UI.Wrapper
{
    public class UserWrapper : WrapperBase<User>
    {
        private ICacheService cacheService;
        private string _pwdErrorMsg;
        private string _branchName;
        public UserWrapper(User model): base(model)
        {

        }

        public UserWrapper(User model, ICacheService cacheService) : base(model)
        {
            this.cacheService = cacheService;
        }
        
        public Int64 Id
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public Int64 BranchId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public string BranchName
        {
            get { return _branchName; }
            set
            {
                _branchName = value;
                OnPropertyChanged();
            }
        }

        public string UserName 
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string DisplayName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Password
        {
            get { return GetValue<string>(); }
            set {
                SetValue(value);
                _pwdErrorMsg = GetFirstError(nameof(Password));
                OnPropertyChanged(nameof(PasswordErrorMessage));
            }
        }
        public string ProfileImage
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool IsAdmin
        {
            get { return GetValue<bool>(); }
            set 
            { 
                SetValue(value); 
                if(!value)
                {
                    CanAccessAllBranch = false;
                }
            }
        }

        public bool IsActive
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool PromptForPasswordReset
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool CanAccessAllBranch
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public DateTime CreatedDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public DateTime? DeactivationDate
        {
            get { return GetValue<DateTime?>(); }
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

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            List<string> errors = new List<string>();
            if (propertyName == "UserName")
            {
                if (string.Equals("sysadmin", UserName, StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add($"Can not create {UserName} user.");
                }
                else
                {
                    if (Id < 1) //Check name only during new user creation
                    {
                        ObservableCollection<UserWrapper> list = cacheService.ReadCache<ObservableCollection<UserWrapper>>(CacheKey.UserList.ToString());

                        if (list != null)
                        {
                            bool exists = list.Any(x => string.Equals(x.UserName, UserName, StringComparison.OrdinalIgnoreCase));
                            if (exists)
                            {
                                errors.Add($"Can not create {UserName} user. It already exists.");
                            }
                        }
                    }
                }

            }
            else if(propertyName == "Password")
            {
                if(Password.Length>10)
                {
                    errors.Add("Password can be upto 10 character long");
                }
            }

            return errors;
        }

    }
}
