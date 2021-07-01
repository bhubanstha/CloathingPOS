﻿using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.Model;
using POSSystem.UI.Enum;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel.Service;
using POSSystem.UI.Views;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private double _popupRightMargin = 200;
        private ICacheService _cacheService;
        private IMessageDialogService _messageDialogService;


        public ICommand UserMenuCommand { get; }
        public ICommand ManageAccount { get; }
        public ICommand AddBranchCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand OpenPdfViewerCommand { get; set; }
        public ICommand ApplicationExitCommand { get; set; }

        public MainWindow MainWindow { get; set; }
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


        private bool _isPopUpMenuVisible = false;

        public bool IsPopUpMenuVisible
        {
            get { return _isPopUpMenuVisible ; }
            set { _isPopUpMenuVisible  = value;
                OnPropertyChanged();
            }
        }

        private bool _isAdminMenuVisible = false;
        private bool _isSysAdminMenuVisible = false;

        public bool IsAdminMenuVisible
        {
            get { return _isAdminMenuVisible; }
            set { _isAdminMenuVisible = value; 
                OnPropertyChanged(); }
        }

        public bool IsSysAdminMenuVisible
        {
            get { return _isSysAdminMenuVisible; }
            set
            {
                _isSysAdminMenuVisible = value;
                OnPropertyChanged();
            }
        }


        public bool IsLogout { get; set; }


        public MainWindowViewModel(ICacheService cacheService, 
            IMessageDialogService messageDialogService)
        {
            _cacheService = cacheService;
            _messageDialogService = messageDialogService;
            User = cacheService.ReadCache<User>("LoginUser");
            UserMenuCommand = new DelegateCommand(OnUserMenuClick);
            ManageAccount = new DelegateCommand(OnManageAccountExecute);
            AddBranchCommand = new DelegateCommand(OnAddBranchExecute);
            SettingsCommand = new DelegateCommand(OnSettingsCommandExecute);
            LogoutCommand = new DelegateCommand(OnUserLogout);
            OpenPdfViewerCommand = new DelegateCommand(OnOpenPdfViewerExecute);
            ApplicationExitCommand = new DelegateCommand(OnApplicationExit);
        }

        private void OnAddBranchExecute()
        {
            Flyout f = StaticContainer.AddBranchFlyout;
            f.IsOpen = !f.IsOpen;
            ManageMenuVisibility();
            StaticContainer.ShowNotification("Test", "this is test", NotificationType.Information);
        }

        private void OnApplicationExit()
        {
            Application.Current.Shutdown();
        }

        private void OnOpenPdfViewerExecute()
        {
            PDFViewerWindow window = new PDFViewerWindow();
            window.Show();
        }

        private void OnUserLogout()
        {
            LoginWindow window = StaticContainer.Container.Resolve<LoginWindow>();
            MainWindow.Hide();
            _cacheService.RemoveCache(CacheKey.LoginUser.ToString());
            window.Show();
            MainWindow.Close();
           
            
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
            StaticContainer.UIHamburgerMenuControl.SelectedOptionsIndex = -1;
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
            IsPopUpMenuVisible = !_isPopUpMenuVisible ;
        }

        public void CheckUserIsAdmin()
        {
            var user = _cacheService.ReadCache<User>("LoginUser");
            if(user != null)
            {
                IsAdminMenuVisible = user.IsAdmin;

                if(string.Equals(user.UserName, "SysAdmin", StringComparison.OrdinalIgnoreCase))
                {
                    IsSysAdminMenuVisible = true;
                }
                else
                {
                    IsSysAdminMenuVisible = false;
                }
            }

        }
    }
}
