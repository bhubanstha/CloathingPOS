﻿using POS.Model;
using System;

namespace POSSystem.UI.Wrapper
{
    public class LoginWrapper : WrapperBase<User>
    {
        public LoginWrapper(User obj) : base(obj)
        {
        }

        public int Id { get { return Model.Id; } }

        public string UserName 
        { 
            get { return GetValue<string>(); }
            set { SetValue(value);}
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

    }
}
