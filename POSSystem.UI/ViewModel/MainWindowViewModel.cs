using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel.Service;
using POSSystem.UI.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace POSSystem.UI.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private ICacheService _cacheService;
        private IMessageDialogService _messageDialogService;
        public IDialogCoordinator _dialogCoordinator;
        private LoginWindow _loginWindow;

        public ICommand UserMenuCommand { get; }
        public ICommand ManageAccount { get; }
        public ICommand SettingsCommand { get; }
        public ICommand LogoutCommand { get; }

        public MetroWindow Window { get; set; }
        public Flyout SettingFlyout { get; set; }
        public User User { get; set; }


        private bool _isUserMenuVisible = false;

        public bool IsUserMenuVisible
        {
            get { return _isUserMenuVisible ; }
            set { _isUserMenuVisible  = value;
                OnPropertyChanged();
            }
        }


        public MainWindowViewModel(ICacheService cacheService, 
            IMessageDialogService messageDialogService,
            IDialogCoordinator dialogCoordinator,
            LoginWindow loginWindow)
        {
            _cacheService = cacheService;
            _messageDialogService = messageDialogService;
            _dialogCoordinator = dialogCoordinator;
            _loginWindow = loginWindow;
            User = cacheService.ReadCache<User>("LoginUser");
            UserMenuCommand = new DelegateCommand(OnUserMenuClick);
            ManageAccount = new DelegateCommand(OnManageAccountExecute);
            SettingsCommand = new DelegateCommand(OnSettingsCommandExecute);
            LogoutCommand = new DelegateCommand(OnUserLogout);
        }

        private void OnUserLogout()
        {
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = _loginWindow;
            Application.Current.MainWindow.Show();
        }

        private void OnSettingsCommandExecute()
        {
            Flyout f = StaticContainer.SettingFlyout;
            f.IsOpen = !f.IsOpen;
            ManageMenuVisibility();
        }


        private void OnManageAccountExecute()
        {

            Window.ShowMessageAsync("This is title", "This is message", MessageDialogStyle.Affirmative);
            //_messageDialogService.ShowDialog("Manage account clicked", Window);
            ManageMenuVisibility(); 
        }


        private void OnUserMenuClick()
        {
            ManageMenuVisibility();
        }

        private void ManageMenuVisibility()
        {
            IsUserMenuVisible = !_isUserMenuVisible ;
        }
    }
}
