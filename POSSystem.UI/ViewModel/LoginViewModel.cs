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

namespace POSSystem.UI.ViewModel
{
    public class LoginViewModel : NotifyPropertyChanged
    {
        public ICommand LoginCommand { get; }
        private IMessageDialogService _dialogService { get; set; }

        private IGenericDataRepository<User> _genericDataRepository;
        private IBouncyCastleEncryption _bouncyCastleEncryption;
        private ICacheService _cacheService;

        public MetroWindow Window { get;  set; }

        public string UserName { get; set; }
        public bool RememberMe { get; set; }

        public LoginViewModel(IMessageDialogService messageDialogService, ICacheService cacheService)
        {
            LoginCommand = new DelegateCommand<object>(OnLoginExecute, OnLoginCanExecute);
            _dialogService = messageDialogService;
            _genericDataRepository = new DataRepository<User>(new POSDataContext());
            _bouncyCastleEncryption = new BouncyCastleEncryption(Encoding.UTF8, new AesEngine());
            _cacheService = cacheService;
        }

        private void OnLoginExecute(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;
            IEnumerable<User> users = _genericDataRepository.GetAll().ToList();
            User u = users
                .Where(f => CompareString(f.UserName, UserName)
                        && f.Password == _bouncyCastleEncryption.EncryptAsAsync(password).Result)
                .FirstOrDefault();
            if (u != null)
            {
                
                _cacheService.SetCache("LoginUser", u);
                MainWindow newWindow = StaticContainer.Container.Resolve<MainWindow>();
                Application.Current.MainWindow = newWindow;
                Window.Close();
                newWindow.Show();
                               
                StaticContainer.ThisApp.MainWindow = newWindow;
               
                //_dialogService.ShowDialog("This is test", Window);
            }
        }

        private bool OnLoginCanExecute(object parameter)
        {
            return true;
        }

        
        
    }
}
