using POS.Model;
using POSSystem.UI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.ViewModel
{
    public class UserProfileViewModel
    {
        private ICacheService _cacheService;
        private bool _isUserNameEditable = false;

        public User CurrentUser { get; private set; }
        
        public bool IsUserNameEditable
        {
            get { return _isUserNameEditable; }
            set { _isUserNameEditable = value; }
        }


        public UserProfileViewModel(ICacheService cacheService)
        {
            _cacheService = cacheService;
            LoadUserDetail();
        }

        private void LoadUserDetail()
        {
            CurrentUser = _cacheService.ReadCache<User>("LoginUser");
        }
    }
}
