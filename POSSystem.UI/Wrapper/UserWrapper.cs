﻿using POS.Model;
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
            set { SetValue(value); }
        }

        public bool IsAdmin
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
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
                    ObservableCollection<UserWrapper> list = cacheService.ReadCache<ObservableCollection<UserWrapper>>(CacheKey.UserList.ToString());

                    bool exists = list.Any(x => string.Equals(x.UserName, UserName, StringComparison.OrdinalIgnoreCase));
                    if (exists)
                    {
                        errors.Add($"Can not create {UserName} user. It already exists.");
                    }
                }

            }

            return errors;
        }

    }
}