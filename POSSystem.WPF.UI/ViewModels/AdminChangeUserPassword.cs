using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.Core.Model;
using POSSystem.WPF.UI.Event;
using POSSystem.WPF.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace POSSystem.WPF.UI.ViewModel
{

    public class ChangePassword
    {

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(10, ErrorMessage = "Password can be 10 characters long.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(10, ErrorMessage = "Password can be 10 characters long.")]
        [Compare(nameof(Password), ErrorMessage = "Confirmation password not matched with Password.")]
        public string ConfirmationPassword { get; set; }

        public bool PromptForPasswordReset { get; set; }

    }

    public class ChangePasswordWrapper : WrapperBase<ChangePassword>
    {
        public ChangePasswordWrapper(ChangePassword obj) : base(obj)
        {

        }

        public string Password 
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string ConfirmationPassword
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool PromptForPasswordReset
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }
    }
    
    
    
    public class AdminChangeUserPasswordViewModel : ViewModelBase
    {
        private ChangePasswordWrapper _newPassword = null;
        private User user;
        private IEventAggregator eventAggregator;

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
        

        public AdminChangeUserPasswordViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
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

                UserBO bO = new UserBO();
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
