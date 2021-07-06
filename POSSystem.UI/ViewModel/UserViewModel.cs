using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities;
using POS.Utilities.Encryption;
using POSSystem.UI.Enum;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Views.Dialog;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private bool _hasChanges;
        private string _buttonText = "Create Account";
        private ObservableCollection<UserWrapper> _userList;
        private UserWrapper _newUser;


        private ICacheService _cacheService;
        private AdminChangePassword _adminChangePasswordUI;
        private IEventAggregator _eventAggregator;
        private IBouncyCastleEncryption _encryption;
        private UserBO _userBo;

        public ICommand CreateUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ResetUserCommand { get; }
        public ICommand RestoreUserCommand { get; }
        public ICommand ChangeUserPasswordCommand { get; }
        public UserWrapper NewUser
        {
            get { return _newUser; }
            set
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserWrapper> UsersList
        {
            get { return _userList; }
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }



        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; OnPropertyChanged(); }
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
                    //((DelegateCommand)CreateUserCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public UserViewModel(ICacheService cacheService, IBouncyCastleEncryption encryption, IEventAggregator eventAggregator, AdminChangePassword adminChangePasswordUI)
        {
            _cacheService = cacheService;
            this._adminChangePasswordUI = adminChangePasswordUI;
            this._eventAggregator = eventAggregator;
            this._encryption = encryption;
            _userBo = new UserBO(encryption);
            NewUser = new UserWrapper(new User(), cacheService)
            {
                PromptForPasswordReset = true,
                UserName = "",
                CanAccessAllBranch = false,
                BranchId = StaticContainer.ActiveBranchId,
                ProfileImage=""
            };


            NewUser.PropertyChanged += NewUser_PropertyChanged;
            CreateUserCommand = new DelegateCommand(OnCreateUserExecute, OnCreateUserCanExecute);
            EditUserCommand = new DelegateCommand<UserWrapper>(EditUser);
            DeleteUserCommand = new DelegateCommand<UserWrapper>(LockUser);
            ResetUserCommand = new DelegateCommand(ResetUser);
            RestoreUserCommand = new DelegateCommand<UserWrapper>(OnUserRestoreExecute);
            ChangeUserPasswordCommand = new DelegateCommand<UserWrapper>(OnUserChangePasswordExecute);
            eventAggregator.GetEvent<UserPasswordChangedEvent>().Subscribe(OnUserPasswordChanged);
            eventAggregator.GetEvent<BranchSwitchedEvent>().Subscribe(OnBranchSwitched);
        }

        private void OnBranchSwitched(BranchWrapper obj)
        {
            LoadAllUsers();
        }

        private void OnUserPasswordChanged(User obj)
        {
            var u = UsersList.Where(x => x.Id == obj.Id).FirstOrDefault();
            u.PromptForPasswordReset = obj.PromptForPasswordReset;
            if (NewUser.Id == obj.Id)
            {
                NewUser.PromptForPasswordReset = obj.PromptForPasswordReset;
            }
        }

        private void OnUserChangePasswordExecute(UserWrapper obj)
        {
            MetroWindow _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            StaticContainer.ChangeUserPasswordDialog = _adminChangePasswordUI;
            _eventAggregator.GetEvent<ChangeUserPasswordEvent>().Publish(obj.Model);
            _window.ShowMetroDialogAsync(_adminChangePasswordUI);
        }


        private async void OnUserRestoreExecute(UserWrapper obj)
        {
            if (obj != null && obj.Id > 0)
            {
                obj.DeactivationDate = null;
                obj.PromptForPasswordReset = true;
                obj.IsActive = true;
                _userBo = new UserBO(_encryption);
                int i = await _userBo.UpdateUser(obj.Model);
                if (i > 0)
                {
                    ManageUserInCollection(obj.Model);
                    StaticContainer.ShowNotification("User Activated", $"User {obj.DisplayName} is re activated on {DateTime.Now.ToString("yyyy/MM/dd")}", NotificationType.Success);
                }
            }
        }

        private void NewUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((DelegateCommand)CreateUserCommand).RaiseCanExecuteChanged();
        }

        private void ResetUser()
        {
            ClearAll();
        }

        private async void LockUser(UserWrapper obj)
        {
            if (obj != null && obj.Id > 0)
            {
                obj.DeactivationDate = DateTime.Now;
                obj.PromptForPasswordReset = false;
                obj.IsActive = false;
                _userBo = new UserBO(_encryption);
                int i = await _userBo.UpdateUser(obj.Model);
                if (i > 0)
                {
                    ManageUserInCollection(obj.Model);
                    //LoadAllUsers();
                    StaticContainer.ShowNotification("User Locked", $"User {obj.DisplayName} is deactivated on {DateTime.Now.ToString("yyyy/MM/dd")}", NotificationType.Success);
                }
            }
        }

        private void EditUser(UserWrapper obj)
        {
            _userBo = new UserBO(_encryption);
            this.NewUser.Id = obj.Id;
            this.NewUser.DisplayName = obj.DisplayName;
            this.NewUser.UserName = obj.UserName;
            this.NewUser.Password = _userBo.DecryptPassword(obj.Password).Result;
            this.NewUser.IsAdmin = obj.IsAdmin;
            this.NewUser.PromptForPasswordReset = obj.PromptForPasswordReset;
            this.NewUser.CanAccessAllBranch = false;
            this.NewUser.BranchId = obj.BranchId;
            this.NewUser.CanAccessAllBranch = obj.CanAccessAllBranch;
            this.NewUser.ProfileImage = obj.ProfileImage;
            this.ButtonText = "Update User";
        }

        private bool OnCreateUserCanExecute()
        {
            return this.NewUser != null && !this.NewUser.HasErrors && !string.IsNullOrEmpty(this.NewUser.Password);
        }

        private async void OnCreateUserExecute()
        {
            try
            {


                User u = new User
                {
                    Id = this.NewUser.Id,
                    BranchId = StaticContainer.ActiveBranchId,
                    UserName = this.NewUser.UserName,
                    DisplayName = this.NewUser.DisplayName,
                    Password = this.NewUser.Password,
                    IsAdmin = this.NewUser.IsAdmin,
                    PromptForPasswordReset = this.NewUser.PromptForPasswordReset,
                    IsActive = true,
                    CanAccessAllBranch = this.NewUser.CanAccessAllBranch,
                    CreatedDate = DateTime.Now,
                    ProfileImage = this.NewUser.ProfileImage
                };

                string title = "";
                string msg = "";
                _userBo = new UserBO(_encryption);
                int id = 0;
                if (u.Id > 0)
                {
                    u.Password = await _userBo.EncryptPassword(u.Password);
                    id = await _userBo.UpdateUser(u);
                    ButtonText = "Create Account";
                    title = "User Updated";
                    msg = $"User {u.UserName} is updated successfully.";
                }
                else
                {
                    id = await _userBo.SaveUser(u);
                    title = "User Creation";
                    msg = $"User {u.UserName} is created successfully.";
                }

                if (id > 0)
                {
                    ClearAll();
                    LoadAllUsers();
                    StaticContainer.NotificationManager.Show(new NotificationContent
                    {
                        Message = "New user is added into the system successfully.",
                        Title = "User Created",
                        Type = NotificationType.Success
                    });
                }
            }
            catch (Exception ex)
            {
                StaticContainer.NotificationManager.Show(new NotificationContent
                {
                    Message = "Could not create user. Please provide all the required values.",
                    Title = "Error",
                    Type = NotificationType.Error
                });
            }

        }

        private void LoadAllUsers()
        {
            User me = _cacheService.ReadCache<User>(CacheKey.LoginUser.ToString());
            _userBo = new UserBO(_encryption);
            List<User> _users = _userBo.GetAllUser(me.UserName, StaticContainer.ActiveBranchId);
            UsersList = new ObservableCollection<UserWrapper>();
            foreach (User u in _users)
            {
                UserWrapper userWrapper = new UserWrapper(u);
                userWrapper.BranchName = u.Branch.BranchName;
                UsersList.Add(userWrapper);
            }
            _cacheService.SetCache(CacheKey.UserList.ToString(), UsersList);
        }

        private void ManageUserInCollection(User obj)
        {
            UserWrapper u = UsersList.Where(x => x.Id == obj.Id).FirstOrDefault();
            if (u != null)
            {
                u.IsActive = obj.IsActive;
                u.DeactivationDate = obj.DeactivationDate;
                u.PromptForPasswordReset = obj.PromptForPasswordReset;
            }
        }

        private void ClearAll()
        {
            this.ButtonText = "Create Account";
            this.NewUser.Id = 0;
            this.NewUser.UserName = "";
            this.NewUser.Password = "";
            this.NewUser.DisplayName = "";
            this.NewUser.IsAdmin = false;
            this.NewUser.PromptForPasswordReset = true;
            this.NewUser.Password = "";
        }



    }
}

