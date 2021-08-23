using log4net;
using Notifications.Wpf;
using POSSystem.UI.Service;
using Prism.Commands;
using SoftwareRegistration;
using System;
using System.Configuration;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class RegistrationViewModel : ViewModelBase
    {
        private ILog _log;
        private string _serialKey;
        private string _message;

        public string SerialKey
        {
            get { return _serialKey; }
            set { _serialKey = value; OnPropertyChanged(); }
        }


        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }



        public ICommand RegisterCommand { get; set; }

        public RegistrationViewModel(ILogger logger)
        {
            _log = logger.GetLogger(typeof(RegistrationViewModel));
            RegisterCommand = new DelegateCommand(OnRegistrationExecute);
        }

        private void OnRegistrationExecute()
        {
            try
            {
                RegistrationService.Register(SerialKey, _log);
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
                _log.Error("OnRegistrationExecute", ex);
            }
        }
    }
}
