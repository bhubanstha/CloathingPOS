using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Enum;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace POSSystem.UI.ViewModel
{
    public class UserBranchViewModel :ViewModelBase
    {
        private User _loggedInUser;
        private ICacheService cacheService;
        private ObservableCollection<BranchWrapper> branches;

        public ObservableCollection<BranchWrapper> Branches
        {
            get { return branches; }
            set
            {
                branches = value;
                OnPropertyChanged();
            }
        }

        public UserBranchViewModel(ICacheService cache, IEventAggregator eventAggregator)
        {
            _loggedInUser = cache.ReadCache<User>(CacheKey.LoginUser.ToString());
            cacheService = cache;
            LoadBranches();
            eventAggregator.GetEvent<BranchChangedEvent>().Subscribe(OnBranchUpdateReceived);
        }

        private void OnBranchUpdateReceived(BranchChangedEventArgs obj)
        {
            if (obj.Action == EventAction.Add)
            {
                if(_loggedInUser == null)
                {
                    _loggedInUser = cacheService.ReadCache<User>(CacheKey.LoginUser.ToString());
                }
                if (_loggedInUser.CanAccessAllBranch)
                {
                    BranchWrapper branchWrapper = new BranchWrapper(new Branch())
                    {
                        Id = obj.Branch.Id,
                        BranchName = obj.Branch.BranchName,
                        BranchAddress = obj.Branch.BranchAddress,
                        ShopId = obj.Branch.ShopId
                    };

                    Branches.Add(branchWrapper);
                }
            }
            else if (obj.Action == EventAction.Update)
            {
                var item = Branches.Where(x => x.Id == obj.Branch.Id).FirstOrDefault();
                item.BranchName = obj.Branch.BranchName;
                item.BranchAddress = obj.Branch.BranchAddress;
            }
            else if (obj.Action == EventAction.Remove)
            {
                var item = Branches.Where(x => x.Id == obj.Branch.Id).FirstOrDefault();
                Branches.Remove(item);
                //if(NewUser.BranchId == obj.Branch.Id)
                //{
                //    NewUser.BranchId == Branches
                //}
            }
        }
        private void LoadBranches()
        {
            BranchBO bo = new BranchBO();
            List<Branch> branchlist = _loggedInUser == null? bo.GetAll() : bo.GetAll(_loggedInUser.BranchId.Value, _loggedInUser.CanAccessAllBranch);
            Branches = new ObservableCollection<BranchWrapper>();
            foreach (Branch branch in branchlist)
            {
                BranchWrapper branchWrapper = new BranchWrapper(new Branch())
                {
                    Id = branch.Id,
                    ShopId = branch.ShopId,
                    BranchName = branch.BranchName,
                    BranchAddress = branch.BranchAddress
                };
                Branches.Add(branchWrapper);
            }

        }
    }
}
