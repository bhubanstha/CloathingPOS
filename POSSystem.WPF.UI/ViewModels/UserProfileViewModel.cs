using POS.Core.Model;
using POSSystem.WPF.UI.Service;

namespace POSSystem.WPF.UI.ViewModel
{
    public class UserProfileViewModel
    {
        private ICacheService _cacheService;
        

        public User CurrentUser { get; private set; }
        
        


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
