﻿using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSSystem.UI.UIModel;
using System.Windows.Input;
using Prism.Commands;
using POS.BusinessRule;
using Notifications.Wpf.Controls;
using Notifications.Wpf;

namespace POSSystem.UI.ViewModel
{
    public class ForgetPasswordViewModel : NotifyPropertyChanged
    {
        private bool _isUserNameEditable = false;
        private ForgetPasswordWrapper _currentUser;
        private ICacheService _cacheService;
        private User _user;
        private UserBO userBO;

        public ICommand ChangePasswordCommand { get; }
        public ForgetPasswordWrapper CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; OnPropertyChanged(); }
        }
        
        public bool IsUserNameEditable
        {
            get { return _isUserNameEditable; }
            set { _isUserNameEditable = value; OnPropertyChanged(); }
        }

        public ForgetPasswordViewModel(ICacheService cacheService)
        {
            _cacheService = cacheService;
            _user = _cacheService.ReadCache<User>("LoginUser");
            ChangePasswordCommand = new DelegateCommand(OnPasswordChange, OnPasswordCanChange);            
            LoadLoginUser();
            CurrentUser.PropertyChanged += CurrentUser_PropertyChanged;
        }

        private void CurrentUser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((DelegateCommand)ChangePasswordCommand).RaiseCanExecuteChanged();
        }

        private async void OnPasswordChange()
        {
            try
            {
                userBO = new UserBO();
                User u = userBO.GetUserFromUserName(CurrentUser.UserName);
                if(u != null)
                {
                    u.Password = await userBO.EncryptPassword( CurrentUser.Password);
                    u.LastPasswordChangeDate = DateTime.Now;
                   int i=  await userBO.UpdateUser(u);
                    if(i>0)
                    {
                        _user.LastPasswordChangeDate = u.LastPasswordChangeDate;
                        _cacheService.SetCache<User>("LoginUser", _user);
                        CurrentUser.Password = string.Empty;
                        CurrentUser.ConfirmPassword = string.Empty;
                        StaticContainer.NotificationManager.Show(new NotificationContent
                        {
                            Title="Password Changed",
                            Message = "User password is changed",
                            Type = NotificationType.Success
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                StaticContainer.NotificationManager.Show(new NotificationContent
                {
                    Title = "Application Error",
                    Message = ex.Message,
                    Type = NotificationType.Error
                });
            }
        }

        private bool OnPasswordCanChange()
        {
            return !string.IsNullOrEmpty(CurrentUser.Password) &&  !string.IsNullOrEmpty(CurrentUser.Password) && !string.IsNullOrEmpty(CurrentUser.ConfirmPassword) && string.Equals(CurrentUser.Password, CurrentUser.ConfirmPassword);
        }

        private void LoadLoginUser()
        {
            if (_user == null)
            {
                CurrentUser = new ForgetPasswordWrapper(new ForgetPasswordModel());
            }
            else
            {
                ForgetPasswordModel model = new ForgetPasswordModel
                {
                    UserName = _user.UserName,
                };
                CurrentUser = new ForgetPasswordWrapper(model);
            }
        }
    }
}