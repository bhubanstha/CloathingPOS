using Autofac;
using log4net;
using MahApps.Metro.Controls;
using POS.BusinessRule;
using POS.Model;
using POS.Model.ViewModel;
using POS.Utilities.Encryption;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private bool _hasChanges;
        private ILog _log;
        private LoginWrapper _loginUser;
        private string _noUserFound;
        private bool _isLoginOnProgress = false;

        public ICommand LoginCommand { get; }
        private IBouncyCastleEncryption _bouncyCastleEncryption;
        private ICacheService _cacheService;

        public MetroWindow LoginWindow { get; set; }

        public bool IsLoginOnProgress
        {
            get { return _isLoginOnProgress; }
            set { _isLoginOnProgress = value; OnPropertyChanged(); }
        }


        public LoginWrapper LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; OnPropertyChanged(); }
        }


        public string NoUserFound
        {
            get { return _noUserFound; }
            set
            {
                _noUserFound = value;
                OnPropertyChanged();
            }
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

        public LoginViewModel(ICacheService cacheService, IBouncyCastleEncryption encryption, ILogger logger)
        {
            _bouncyCastleEncryption = encryption;
            _cacheService = cacheService;
            _log = logger.GetLogger(typeof(LoginViewModel));
            LoginUser = new LoginWrapper(new LoginModel());
            LoginUser.UserName = "";
            LoginUser.PropertyChanged += LoginUser_PropertyChanged;
            LoginCommand = new DelegateCommand(OnLoginExecute, OnLoginCanExecute);
        }

        private void LoginUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
        }

        private async void OnLoginExecute()
        {
            IsLoginOnProgress = true;
            MetroWindow window = await Login();
            if (window != null)
            {
                if (window is ForgotPasswordWindow)
                {

                    LoginWindow.Hide();
                    bool? result = window.ShowDialog();

                    if (result.HasValue && result.Value == true)
                    {
                        IsLoginOnProgress = false;
                        LoginUser.Password = "";
                        LoginWindow.Show();
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else if (window is MainWindow)
                {

                    LoginWindow.Hide();
                    window.Show();
                    LoginWindow.Close();
                }
            }
            else
            {
                IsLoginOnProgress = false;
                NoUserFound = "Username or password is invalid";
            }
        }


        private Task<MetroWindow> Login()
        {
            try
            {
                Task<MetroWindow> t = Task.Run(() =>
                    {
                        string encryptePassword = _bouncyCastleEncryption.EncryptAsAsync(LoginUser.Password).Result;
                        UserBO bo = new UserBO(_bouncyCastleEncryption);

                        User u = bo.Login(LoginUser.UserName, encryptePassword, LoginUser.BranchId);

                        MetroWindow window = null;
                        if (u != null)
                        {
                            if (LoginUser.RememberMe)
                            {
                                Application.Current.Properties["UserName"] = LoginUser.UserName;
                                Application.Current.Properties["RememberMe"] = LoginUser.RememberMe;
                                Application.Current.Properties["BranchId"] = LoginUser.BranchId;
                            }
                            else
                            {
                                Application.Current.Properties["UserName"] = "";
                                Application.Current.Properties["RememberMe"] = false;
                                Application.Current.Properties["BranchId"] = -1;
                            }
                            u.BranchId = LoginUser.BranchId;
                            _cacheService.SetCache(Enum.CacheKey.LoginUser.ToString(), u);

                            if (u != null)
                            {
                                if (u.PromptForPasswordReset)
                                {
                                    Application.Current.Dispatcher.Invoke(new System.Action(() =>
                                    {
                                        LoginWindow.Hide();
                                        StaticContainer.IsPasswordChanged = false;
                                        ForgotPasswordWindow forgetPasswindow = null;
                                        forgetPasswindow = GetPasswordResetWindow();
                                        StaticContainer.ThisApp.MainWindow = forgetPasswindow;
                                        forgetPasswindow.IsUserNameEditable = false;
                                        forgetPasswindow.IsBackButtonVisible = false;
                                        forgetPasswindow.LookForPasswordChange = true;

                                        window = forgetPasswindow;
                                    }));

                                }
                                else
                                {
                                    BranchBO branchBO = new BranchBO();
                                    Branch b = branchBO.GetById(u.BranchId.Value);
                                    if (b != null)
                                    {
                                        StaticContainer.Shop = new ShopVM
                                        {
                                            Id = b.Shop.Id,
                                            Address = b.BranchAddress,
                                            CalculateVATOnSales = b.Shop.CalculateVATOnSales,
                                            LogoPath = b.Shop.LogoPath,
                                            Name = b.Shop.Name,
                                            PANNumber = b.Shop.PANNumber,
                                            PdfPassword = b.Shop.PdfPassword,
                                            PrintInvoice = b.Shop.PrintInvoice
                                        };
                                    }

                                    Application.Current.Dispatcher.Invoke(new System.Action(() =>
                                    {
                                        MainWindow mainWindow = GetMainWindow();
                                        StaticContainer.ThisApp.MainWindow = mainWindow;
                                        window = mainWindow;
                                    }));
                                }
                            }
                            return window;

                        }

                        return window;
                    });
                return t;
            }
            catch (System.Exception ex)
            {
                _log.Error("Login", ex);
            }
            return null;
        }



        private bool OnLoginCanExecute()
        {
            return !string.IsNullOrEmpty(LoginUser.UserName) && !string.IsNullOrEmpty(LoginUser.Password) && LoginUser.BranchId > 0;
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

    }
}
