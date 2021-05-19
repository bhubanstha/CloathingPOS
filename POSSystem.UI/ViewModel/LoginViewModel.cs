using MahApps.Metro.Controls;
using Org.BouncyCastle.Crypto.Engines;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POS.Utilities.Encryption;
using POSSystem.UI.ViewModel.Service;
using POSSystem.UI.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.Caching;
using POSSystem.UI.Service;
using System.Windows;
using Autofac;
using POSSystem.UI.Wrapper;
using POSSystem.UI.UIModel;

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

        public MetroWindow Window { get;  set; }


        

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
                
                _cacheService.SetCache("LoginUser", u);
                MainWindow newWindow = StaticContainer.Container.Resolve<MainWindow>();
                Application.Current.MainWindow = newWindow;
                Window.Close();
                newWindow.Show();
                               
                StaticContainer.ThisApp.MainWindow = newWindow;
            }
        }

        private bool OnLoginCanExecute()
        {
            return !string.IsNullOrEmpty(LoginUser.UserName) && !string.IsNullOrEmpty(LoginUser.Password);
        }

        
        
    }
}
