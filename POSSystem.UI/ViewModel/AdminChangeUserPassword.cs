using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities.Encryption;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{

    public class ChangePassword
    {

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(10, ErrorMessage = "Password can be 10 characters long.")]
        [Compare("ConfirmationPassword", ErrorMessage = "Password and confirmation password didn't match.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(10, ErrorMessage = "Password can be 10 characters long.")]
        [Compare("Password", ErrorMessage = "Confirmation password and  password didn't match.")]
        public string ConfirmationPassword { get; set; }

        public bool PromptForPasswordReset { get; set; }

    }

    public class ChangePasswordWrapper : WrapperBase<ChangePassword>
    {
        private string _pwdMsg;
        private string _conMsg;
        public ChangePasswordWrapper(ChangePassword obj) : base(obj)
        {

        }

        public string Password 
        {
            get { return GetValue<string>(); }
            set 
            { 
                SetValue(value);
                ClearErrors(nameof(ConfirmationPassword));
                PasswordMessage = GetFirstError(nameof(Password));
                ConfirmPasswordMessage = GetFirstError(nameof(ConfirmationPassword));

                OnPropertyChanged(nameof(PasswordMessage));
                OnPropertyChanged(nameof(ConfirmPasswordMessage));
            }
        }

        public string ConfirmationPassword
        {
            get { return GetValue<string>(); }
            set 
            { 
                SetValue(value);
                ClearErrors(nameof(Password));
                ConfirmPasswordMessage = GetFirstError(nameof(ConfirmationPassword));
                PasswordMessage = GetFirstError(nameof(Password));

                OnPropertyChanged(nameof(ConfirmPasswordMessage));
                OnPropertyChanged(nameof(PasswordMessage));
            }
        }

        public bool PromptForPasswordReset
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
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
            get { return _conMsg; }
            set
            {
                _conMsg = value;
                OnPropertyChanged();
            }
        }
    }
    
    
    
    public class AdminChangeUserPasswordViewModel : ViewModelBase
    {
        private ChangePasswordWrapper _newPassword = null;
        private User user;
        private IEventAggregator eventAggregator;
        private IBouncyCastleEncryption _encryption;

        public ChangePasswordWrapper NewPassword 
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged();
               
            }

        }

       

        public ICommand ChangePasswordCommand { get; }
        public ICommand CloseDialogCommand { get; }
        

        public AdminChangeUserPasswordViewModel(IEventAggregator eventAggregator, IBouncyCastleEncryption encryption)
        {
            this.eventAggregator = eventAggregator;
            this._encryption = encryption;
            NewPassword = new ChangePasswordWrapper(new ChangePassword());
            ChangePasswordCommand = new DelegateCommand(OnChangePasswordExecute, CanChangePasswordExecute);
            CloseDialogCommand = new DelegateCommand(OnCloseDialogExecute);
            eventAggregator.GetEvent<ChangeUserPasswordEvent>().Subscribe(OnDialogOpen);

            NewPassword.PropertyChanged += NewPassword_PropertyChanged;

        }

        private void NewPassword_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((DelegateCommand)ChangePasswordCommand).RaiseCanExecuteChanged();
        }

        private bool CanChangePasswordExecute()
        {
            return !string.IsNullOrEmpty(NewPassword.Password) && !string.IsNullOrEmpty(NewPassword.ConfirmationPassword) && string.Equals(NewPassword.Password, NewPassword.ConfirmationPassword);
        }

        private void OnDialogOpen(User obj)
        {
            this.user = obj;
            NewPassword.Password = "";
            NewPassword.ConfirmationPassword = "";
            NewPassword.PromptForPasswordReset = obj.PromptForPasswordReset;
        }

        private void OnCloseDialogExecute()
        {
            MetroWindow _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            _window.HideMetroDialogAsync(StaticContainer.ChangeUserPasswordDialog);
        }

        private async void OnChangePasswordExecute()
        {
            try
            {

                UserBO bO = new UserBO(_encryption);
                user.Password = await bO.EncryptPassword(NewPassword.Password);
                user.PromptForPasswordReset = NewPassword.PromptForPasswordReset;
                await bO.UpdateUser(user);
                OnCloseDialogExecute();
                eventAggregator.GetEvent<UserPasswordChangedEvent>().Publish(user);
                StaticContainer.ShowNotification("Password Changed", $"{user.DisplayName} login password changed.", Notifications.Wpf.NotificationType.Success);
            }
            catch (Exception)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, Notifications.Wpf.NotificationType.Error);
                throw;
            }
        }
    }
}
