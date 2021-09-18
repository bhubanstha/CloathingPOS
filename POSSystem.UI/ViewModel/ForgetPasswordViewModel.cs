using log4net;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Model.ViewModel;
using POS.Utilities.Encryption;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class ForgetPasswordViewModel : NotifyPropertyChanged
    {
        private bool _isUserNameEditable = false;
        private ILog _log;
        private ForgetPasswordWrapper _currentUser;
        private ICacheService _cacheService;
        private IBouncyCastleEncryption _encryption;
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

        public ForgetPasswordViewModel(ICacheService cacheService, IBouncyCastleEncryption encryption, ILogger logger)
        {
            _cacheService = cacheService;
            this._encryption = encryption;
            this._log = logger.GetLogger(typeof(ForgetPasswordViewModel));
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
                userBO = new UserBO(_encryption);
                User u = await userBO.GetUserFromUserName(CurrentUser.UserName);
                if (u != null)
                {
                    u.Password = await userBO.EncryptPassword(CurrentUser.Password);
                    u.LastPasswordChangeDate = DateTime.Now;
                    u.PromptForPasswordReset = false;
                    int i = await userBO.UpdateUser(u);
                    if (i > 0)
                    {
                        if (_user != null)
                        {
                            _user.LastPasswordChangeDate = u.LastPasswordChangeDate;
                            _cacheService.SetCache<User>("LoginUser", _user);
                        }
                        CurrentUser.Password = null;
                        CurrentUser.ConfirmPassword = null;
                        if (IsUserNameEditable)
                        {
                            CurrentUser.UserName = null;
                        }
                        StaticContainer.IsPasswordChanged = true;
                        StaticContainer.ShowNotification("Password Changed", "User password is changed", NotificationType.Success);
                    }
                }
                else
                {
                    StaticContainer.ShowNotification("No User", "User doesn't exists to change password", NotificationType.Information);
                }

            }
            catch (Exception ex)
            {
                _log.Error("OnPasswordChange", ex);
                StaticContainer.IsPasswordChanged = false;
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
        }

        private bool OnPasswordCanChange()
        {
            return !string.IsNullOrEmpty(CurrentUser.Password) && !string.IsNullOrEmpty(CurrentUser.Password) && !string.IsNullOrEmpty(CurrentUser.ConfirmPassword) && string.Equals(CurrentUser.Password, CurrentUser.ConfirmPassword);
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
