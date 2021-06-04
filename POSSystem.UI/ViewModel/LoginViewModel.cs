using Autofac;
using MahApps.Metro.Controls;
using Org.BouncyCastle.Crypto.Engines;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POS.Utilities.Encryption;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class LoginViewModel : NotifyPropertyChanged
    {
        private bool _hasChanges;
        private LoginWrapper _loginUser;

        public ICommand LoginCommand { get; }
        private IGenericDataRepository<User> _genericDataRepository;
        private IBouncyCastleEncryption _bouncyCastleEncryption;
        private ICacheService _cacheService;

        public MetroWindow LoginWindow { get;  set; }


        

        public LoginWrapper LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; OnPropertyChanged(); }
        }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public LoginViewModel( ICacheService cacheService)
        {
            
            _genericDataRepository = new DataRepository<User>(new POSDataContext());
            _bouncyCastleEncryption = new BouncyCastleEncryption(Encoding.UTF8, new AesEngine());
            _cacheService = cacheService;
            LoginUser = new LoginWrapper(new LoginModel());
            LoginUser.UserName = "";
            LoginUser.PropertyChanged += LoginUser_PropertyChanged;
            LoginCommand = new DelegateCommand(OnLoginExecute, OnLoginCanExecute);
        }

        private void LoginUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
            //if(e.PropertyName == nameof(LoginUser.HasErrors))
            //{
            //    ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
            //}
        }

        private void OnLoginExecute()
        {
            IEnumerable<User> users = _genericDataRepository.GetAll().ToList();
            User u = users
                .Where(f => CompareString(f.UserName, LoginUser.UserName)
                        && f.Password == _bouncyCastleEncryption.EncryptAsAsync(LoginUser.Password).Result)
                .FirstOrDefault();
            if (u != null)
            {
                if(LoginUser.RememberMe)
                {
                    Application.Current.Properties["UserName"] = LoginUser.UserName;
                    Application.Current.Properties["RememberMe"] = LoginUser.RememberMe;
                }
                else
                {
                    Application.Current.Properties["UserName"] = "";
                    Application.Current.Properties["RememberMe"] = false;
                }
                
                _cacheService.SetCache("LoginUser", u);
                
                if(u.PromptForPasswordReset)
                {
                    LoginWindow.Hide();
                    ForgotPasswordWindow forgetPasswindow = null;
                    forgetPasswindow = GetPasswordResetWindow();
                    ManageWindowVisibility(forgetPasswindow,  null);
                    forgetPasswindow.IsUserNameEditable = false;
                    forgetPasswindow.IsBackButtonVisible = false;
                    forgetPasswindow.LookForPasswordChange = true;
                    bool? result = forgetPasswindow.ShowDialog();
                    if(result.HasValue && result.Value == true)
                    {
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                        
                    }
                }
                else
                {
                    MainWindow mainWindow = GetMainWindow();
                    ManageWindowVisibility(mainWindow, LoginWindow, true);
                }
                
            }
        }

        private bool OnLoginCanExecute()
        {
            return !string.IsNullOrEmpty(LoginUser.UserName) && !string.IsNullOrEmpty(LoginUser.Password);
        }

        
        private MainWindow GetMainWindow()
        {
            MainWindow window = StaticContainer.Container.Resolve<MainWindow>();
            return window;
        }
        private ForgotPasswordWindow GetPasswordResetWindow()
        {
            ForgotPasswordWindow window = StaticContainer.Container.Resolve<ForgotPasswordWindow>();
            return window;
        }

        private void ManageWindowVisibility(MetroWindow windowToSetMainWindow, MetroWindow windowToClose, bool show=false)
        {
            Application.Current.MainWindow = windowToSetMainWindow;
            if (windowToClose != null)
            {
                windowToClose.Close();
            }
            if (show)
            {
                windowToSetMainWindow.Show();
            }
           
            StaticContainer.ThisApp.MainWindow = windowToSetMainWindow;           
        }
        
    }
}
