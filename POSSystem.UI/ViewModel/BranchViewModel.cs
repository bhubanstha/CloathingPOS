using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class BranchViewModel : ViewModelBase
    {
        private BranchWrapper branch;
        private ObservableCollection<BranchWrapper> branches;
        private IEventAggregator eventAggregator;
        private string buttonText = "Create Branch";

        public BranchWrapper Branch
        {
            get { return branch; }
            set { branch = value; }
        }

        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BranchWrapper> Branches
        {
            get { return branches; }
            set
            {
                branches = value;
                OnPropertyChanged();
            }
        }


        public ICommand SaveBranchCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand EditBranchCommand { get; private set; }
        public ICommand DeleteBranchCommand { get; private set; }


        public BranchViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Branch = new BranchWrapper(new Branch())
            {
                ShopId = 1
            };
            SaveBranchCommand = new DelegateCommand(OnSaveBranchExecute);
            ResetCommand = new DelegateCommand(OnResetExecute);
            EditBranchCommand = new DelegateCommand<BranchWrapper>(OnBranchEditExecute);
            DeleteBranchCommand = new DelegateCommand<BranchWrapper>(OnBranchDeleteExecute);

            LoadBranches();
        }

      

        private async void OnSaveBranchExecute()
        {
            try
            {
                BranchBO bo = new BranchBO();
                Branch branch = new Branch
                {
                    Id = Branch.Id,
                    ShopId = Branch.ShopId,
                    BranchName = Branch.BranchName,
                    BranchAddress = Branch.BranchAddress
                };

                BranchChangedEventArgs args = await ManageBranch(branch, bo);
                string msg = args.Action == EventAction.Add ?
                    "New Branch Added Successfully."
                    : "Branch Changed Successfully.";
                
                eventAggregator.GetEvent<BranchChangedEvent>().Publish(args);
                StaticContainer.ShowNotification("Branch", msg, Notifications.Wpf.NotificationType.Success);
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, Notifications.Wpf.NotificationType.Error);
            }
            finally
            {
                OnResetExecute();
            }
        }

        private void OnResetExecute()
        {
            this.Branch.BranchName = "";
            this.Branch.BranchAddress = "";
            this.Branch.Id = 0;
            ButtonText = "Create Branch";
        }


        private void OnBranchEditExecute(BranchWrapper obj)
        {
            if (obj != null)
            {
                this.Branch.Id = obj.Id;
                this.Branch.BranchName = obj.BranchName;
                this.Branch.BranchAddress = obj.BranchAddress;
                this.Branch.ShopId = obj.ShopId;
                ButtonText = "Update Branch";
            }
        }

        private async void OnBranchDeleteExecute(BranchWrapper obj)
        {
            try
            {
                if (obj != null)
                {
                    BranchBO bO = new BranchBO();
                    int i = await bO.Delete(obj.Id);
                    if (i > 0)
                    {
                        BranchChangedEventArgs args = new BranchChangedEventArgs
                        {
                            Branch = obj.Model,
                            Action = EventAction.Remove
                        };
                        RemoveBranchFromList(args.Branch);
                        eventAggregator.GetEvent<BranchChangedEvent>().Publish(args);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
        }

        private async Task<BranchChangedEventArgs> ManageBranch(Branch branch, BranchBO bO)
        {
            BranchChangedEventArgs args = new BranchChangedEventArgs
            {
                Branch = branch
            };
            if (branch.Id > 0)
            {
                await bO.Update(branch);
                args.Action = EventAction.Update;
            }
            else
            {
                await bO.Save(branch);
                args.Action = EventAction.Add;
            }
            ManageBranchInList(args);
            return args;
        }

        private void ManageBranchInList(BranchChangedEventArgs args)
        {
            if (args.Action == EventAction.Add)
            {
                AddNewBranchToList(args.Branch);
            }
            else if (args.Action == EventAction.Update)
            {
                UpdateBranchInList(args.Branch);
            }
            else
            {
                RemoveBranchFromList(args.Branch);
            }
        }

        private void AddNewBranchToList(Branch obj)
        {
            BranchWrapper branchWrapper = new BranchWrapper(obj);
            Branches.Add(branchWrapper);
        }

        private void UpdateBranchInList(Branch obj)
        {
            var item = Branches.Where(x => x.Id == obj.Id).FirstOrDefault();
            item.BranchName = obj.BranchName;
            item.BranchAddress = obj.BranchAddress;
        }

        private void RemoveBranchFromList(Branch obj)
        {
            var item = Branches.Where(x => x.Id == obj.Id).FirstOrDefault();
            Branches.Remove(item);
        }

        private void LoadBranches()
        {
            BranchBO bo = new BranchBO();
            List<Branch> branchlist = bo.GetAll();
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
