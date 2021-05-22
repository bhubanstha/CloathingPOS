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
        private double _popupRightMargin = 200;
        private ICacheService _cacheService;
        private IMessageDialogService _messageDialogService;
        public IDialogCoordinator _dialogCoordinator;
        public LoginWindow LoginWindow { get; set; }

        public ICommand UserMenuCommand { get; }
        public ICommand ManageAccount { get; }
        public ICommand SettingsCommand { get; }
        public ICommand LogoutCommand { get; }

        public MetroWindow Window { get; set; }
        public Flyout SettingFlyout { get; set; }
        public User User { get; set; }
        public double PopupRightMargin {
            get { return _popupRightMargin; }
            set
            {
                _popupRightMargin = value;
                OnPropertyChanged();
            }
        }


        private bool _isUserMenuVisible = false;

        public bool IsUserMenuVisible
        {
            get { return _isUserMenuVisible ; }
            set { _isUserMenuVisible  = value;
                OnPropertyChanged();
            }
        }

        public bool IsLogout { get; set; }


        public MainWindowViewModel(ICacheService cacheService, 
            IMessageDialogService messageDialogService,
            IDialogCoordinator dialogCoordinator)
        {
            _cacheService = cacheService;
            _messageDialogService = messageDialogService;
            _dialogCoordinator = dialogCoordinator;
            User = cacheService.ReadCache<User>("LoginUser");
            UserMenuCommand = new DelegateCommand(OnUserMenuClick);
            ManageAccount = new DelegateCommand(OnManageAccountExecute);
            SettingsCommand = new DelegateCommand(OnSettingsCommandExecute);
            LogoutCommand = new DelegateCommand(OnUserLogout);
        }

        private void OnUserLogout()
        {
            IsLogout = true;
            Application.Current.MainWindow = LoginWindow;
            Window.Close();            
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
            //StaticContainer.UIHamburgerMenuControl.Items.
            StaticContainer.UIHamburgerMenuControl.Content = StaticContainer.UIHamburgerMenuControl.Items[7];
            StaticContainer.UIHamburgerMenuControl.SelectedIndex = -1;
            //Window.ShowMessageAsync("This is title", "This is message", MessageDialogStyle.Affirmative);
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
