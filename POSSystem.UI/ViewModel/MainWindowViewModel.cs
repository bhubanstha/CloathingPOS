﻿using MahApps.Metro.Controls;
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
using System.Windows.Input;
using System.Windows.Shapes;

namespace POSSystem.UI.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private ICacheService _cacheService;
        private IMessageDialogService _messageDialogService;
        private UserMenuPopupControl _popuMenu;
        public ICommand TestCommand { get; }
        public ICommand ManageAccount { get; }

        public MetroWindow Window { get; set; }
        public User User { get; set; }

        private Visibility _userManuVisibility;

        public Visibility UserMenuVisibility
        {
            get { return _userManuVisibility; }
            set { 
                _userManuVisibility = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(ICacheService cacheService, IMessageDialogService messageDialogService)
        {
            UserMenuVisibility = Visibility.Visible;
            _cacheService = cacheService;
            _messageDialogService = messageDialogService;
            User = cacheService.ReadCache<User>("LoginUser");
            TestCommand = new DelegateCommand<UserMenuPopupControl>(OnLoginExecute, OnLoginCanExecute);
            ManageAccount = new DelegateCommand(OnManageAccountExecute, OnManageAccountCanExecute);
        }

        private void OnManageAccountExecute()
        {
            _messageDialogService.ShowDialog("Manage account clicked", Window);
            ManageMenuVisibility(); 
        }

        private bool OnManageAccountCanExecute()
        {
            return true;
        }

        private void OnLoginExecute(UserMenuPopupControl UserMenuPopup)
        {
            _popuMenu = UserMenuPopup;
            ManageMenuVisibility();
            //_messageDialogService.ShowDialog(UserMenuVisibility.ToString(), Window);
        }

        private bool OnLoginCanExecute(UserMenuPopupControl UserMenuPopup)
        {
            return true;
        }

        private void ManageMenuVisibility()
        {
            UserMenuVisibility = UserMenuVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            _popuMenu.Visibility = UserMenuVisibility;
        }
    }
}
