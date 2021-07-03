using Autofac;
using MahApps.Metro.Controls;
using Org.BouncyCastle.Crypto.Engines;
using POS.BusinessRule;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POS.Model.ViewModel;
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
    public class LoginViewModel : ViewModelBase
    {
        private bool _hasChanges;
        private LoginWrapper _loginUser;
        private string _noUserFound;

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

        

        public string NoUserFound
        {
            get { return _noUserFound; }
            set { _noUserFound = value; 
                OnPropertyChanged(); }
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
        }

        private async void OnLoginExecute()
        {
            string encryptePassword = await _bouncyCastleEncryption.EncryptAsAsync(LoginUser.Password);
            User u = _genericDataRepository
                        .GetAll()
                        .Where(f => f.UserName == LoginUser.UserName
                                && f.Password == encryptePassword
                                && (f.BranchId == LoginUser.BranchId || f.CanAccessAllBranch == true)
                                )
                        .FirstOrDefault();
            if (u != null)
            {
                if(LoginUser.RememberMe)
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
                _cacheService.SetCache("LoginUser", u);
                
                if(u.PromptForPasswordReset)
                {
                    LoginWindow.Hide();
                    ForgotPasswordWindow forgetPasswindow = null;
                    forgetPasswindow =  GetPasswordResetWindow() ;
                    ManageWindowVisibility(forgetPasswindow,  null);
                    forgetPasswindow.IsUserNameEditable = false;
                    forgetPasswindow.IsBackButtonVisible = false;
                    forgetPasswindow.LookForPasswordChange = true;
                    bool? result = forgetPasswindow.ShowDialog();

                    if (result.HasValue && result.Value == true)
                    {
                        LoginWindow.Show();
                        //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        //Application.Current.Shutdown();

                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    BranchBO bo = new BranchBO();
                    Branch b = bo.GetById(u.BranchId.Value);
                    if(b != null)
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
                    MainWindow mainWindow = GetMainWindow();
                    ManageWindowVisibility(mainWindow, LoginWindow, true);
                }
                
            }
            else
            {
                NoUserFound = "Username or password is invalid";
            }
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
