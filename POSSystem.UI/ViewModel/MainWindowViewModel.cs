using Autofac;
using MahApps.Metro.Controls;
using Notifications.Wpf;
using POS.Model;
using POSSystem.UI.Enum;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using POSSystem.UI.Views;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private double _popupRightMargin = 200;
        private ICacheService _cacheService;
        private bool _isPopUpMenuVisible = false;
        private bool _isFeatureHighlightOpen = true;
        private bool _isAdminMenuVisible = false;
        private bool _isSysAdminMenuVisible = false;


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

        public Int64 CurrentBranchId
        {
            get { return StaticContainer.ActiveBranchId; }
            set {
                StaticContainer.ActiveBranchId = value;
                OnPropertyChanged();
            }
        }


        public bool IsPopUpMenuVisible
        {
            get { return _isPopUpMenuVisible ; }
            set { _isPopUpMenuVisible  = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsFeatureHighlightOpen
        {
            get { return _isFeatureHighlightOpen; }
            set {
                _isFeatureHighlightOpen = value;
                OnPropertyChanged();
            }
        }

       

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


        public MainWindowViewModel(ICacheService cacheService)
        {
            CurrentBranchId = _loggedInUser.BranchId.Value;
            _cacheService = cacheService;

            IsFeatureHighlightOpen = Application.Current.Properties["FeatureShown"] == null ? true: Convert.ToBoolean(Application.Current.Properties["FeatureShown"]);
            User = cacheService.ReadCache<User>(CacheKey.LoginUser.ToString());
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
            ManagePopUpMenuVisibility();
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
            ManagePopUpMenuVisibility();
        }


        private void OnManageAccountExecute()
        {
            //StaticContainer.UIHamburgerMenuControl.Items.
            StaticContainer.UIHamburgerMenuControl.Content = StaticContainer.UIHamburgerMenuControl.Items[StaticContainer.UIHamburgerMenuControl.Items.Count-2];
            StaticContainer.UIHamburgerMenuControl.SelectedIndex = -1;
            StaticContainer.UIHamburgerMenuControl.SelectedOptionsIndex = -1;
            //Window.ShowMessageAsync("This is title", "This is message", MessageDialogStyle.Affirmative);
            //_messageDialogService.ShowDialog("Manage account clicked", Window);
            ManagePopUpMenuVisibility(); 
        }


        private void OnUserMenuClick()
        {
            ManagePopUpMenuVisibility();
        }

        private void ManagePopUpMenuVisibility()
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

        public void UpdateBranchOnEdit(Int64 branchId, string branchName)
        {
            if (StaticContainer.ActiveBranchId != branchId)
            {
                CurrentBranchId = branchId;
                StaticContainer.ShowNotification("Branch Switch", $"You have been swtitched to {branchName}.", NotificationType.Information);
            }
        }
    }
}
