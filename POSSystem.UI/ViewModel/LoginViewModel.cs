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
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class LoginViewModel 
    {
        public System.Windows.Input.ICommand LoginCommand { get; }
        public IMessageDialogService _dialogService { get; set; }

        private IGenericDataRepository<User> _genericDataRepository;
        private IBouncyCastleEncryption _bouncyCastleEncryption;

        public MetroWindow Window { get;  set; }
        public LoginViewModel(IMessageDialogService messageDialogService)
        {
            LoginCommand = new DelegateCommand(OnLoginExecute, OnLoginCanExecute);
            _dialogService = messageDialogService;
            _genericDataRepository = new DataRepository<User>(new POSDataContext());
            _bouncyCastleEncryption = new BouncyCastleEncryption(Encoding.UTF8, new AesEngine());
            //Window = window;
        }

        private void OnLoginExecute()
        {
            IEnumerable<User> users = _genericDataRepository.GetAll().ToList();
            var u = users
                .Where(f => f.UserName == "sysadmin" && f.Password == _bouncyCastleEncryption.EncryptAsAsync("123").Result).FirstOrDefault();
            if (u != null)
            {
                _dialogService.ShowDialog("This is test", Window);
            }
        }

        private bool OnLoginCanExecute()
        {
            return true;
        }

        
    }
}
