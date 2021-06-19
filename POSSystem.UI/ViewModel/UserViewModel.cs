using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities;
using POSSystem.UI.Enum;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private bool _hasChanges;
        private string _buttonText = "Create Account";
        private ObservableCollection<UserWrapper> _userList;
        private UserWrapper _newUser;
        private string _logoFullPathName;
        private string _logoName;
        private bool _isLogoChanged = false;


        public PasswordBox PasswordTextBox { get; set; }

        private ICacheService _cacheService;
        private AdminChangePassword _adminChangePasswordUI;
        private IEventAggregator _eventAggregator;
        private UserBO _userBo;

        public ICommand CreateUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ResetUserCommand { get; }
        public ICommand RestoreUserCommand { get; }
        public ICommand FilePickCommand { get; }
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
        public string LogoFullPathName
        {
            get { return _logoFullPathName; }
            set
            {
                _logoFullPathName = value;
                OnPropertyChanged();
            }
        }



        public string LogoName
        {
            get { return _logoName; }
            set { _logoName = value; OnPropertyChanged(); }
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


        public UserViewModel(ICacheService cacheService, IEventAggregator eventAggregator, AdminChangePassword adminChangePasswordUI)
        {
            _cacheService = cacheService;
            this._adminChangePasswordUI = adminChangePasswordUI;
            this._eventAggregator = eventAggregator;
            _userBo = new UserBO();
            NewUser = new UserWrapper(new User(), cacheService);
            NewUser.PromptForPasswordReset = true;

            NewUser.PropertyChanged += NewUser_PropertyChanged;
            CreateUserCommand = new DelegateCommand<PasswordBox>(OnCreateUserExecute).ObservesProperty(() => HasChanges);
            EditUserCommand = new DelegateCommand<UserWrapper>(EditUser);
            DeleteUserCommand = new DelegateCommand<UserWrapper>(DeleteUser);
            ResetUserCommand = new DelegateCommand<PasswordBox>(ResetUser);
            RestoreUserCommand = new DelegateCommand<UserWrapper>(OnUserRestoreExecute);
            FilePickCommand = new DelegateCommand(OnFilePickCommandExecute);
            ChangeUserPasswordCommand = new DelegateCommand<UserWrapper>(OnUserChangePasswordExecute);
            eventAggregator.GetEvent<UserPasswordChangedEvent>().Subscribe(OnUserPasswordChanged);
            LoadAllUsers();
        }

        private void OnUserPasswordChanged(User obj)
        {
            var u = UsersList.Where(x => x.Id == obj.Id).FirstOrDefault();
            u.PromptForPasswordReset = obj.PromptForPasswordReset;
            if(NewUser.Id == obj.Id)
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

        private void OnFilePickCommandExecute()
        {
            string fileName = FileUtility.OpenImageFilePicker();
            if(!string.IsNullOrEmpty(fileName))
            {
                NewUser.ProfileImage = Path.GetFileName(fileName);
                LogoName = $".....\\{NewUser.ProfileImage}";
                LogoFullPathName = fileName;
                _isLogoChanged = true;
            }
        }

        private async void OnUserRestoreExecute(UserWrapper obj)
        {
            if (obj != null && obj.Id > 0)
            {
                obj.DeactivationDate = null;
                obj.PromptForPasswordReset = true;
                obj.IsActive = true;
                UserBO userBO = new UserBO();
                int i = await userBO.UpdateUser(obj.Model);
                if (i > 0)
                {
                    ManageUserInCollection(obj.Model);
                    //LoadAllUsers();
                    StaticContainer.ShowNotification("User Activated", $"User {obj.DisplayName} is re activated on {DateTime.Now.ToString("yyyy/MM/dd")}", NotificationType.Success);
                }
            }
        }

        private void NewUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _userBo.HasChanges();
            }
            //if(e.PropertyName == nameof(NewUser.HasErrors))
            //{
            //    ((DelegateCommand)CreateUserCommand).RaiseCanExecuteChanged();
            //}
        }

        private void ResetUser(PasswordBox obj)
        {
            this.PasswordTextBox = obj;
            ClearAll();
        }

        private async void DeleteUser(UserWrapper obj)
        {
            if (obj != null && obj.Id > 0)
            {
                obj.DeactivationDate = DateTime.Now;
                obj.PromptForPasswordReset = false;
                obj.IsActive = false;
                UserBO userBO = new UserBO();
                int i = await userBO.UpdateUser(obj.Model);
                if (i > 0)
                {
                    ManageUserInCollection(obj.Model);
                    //LoadAllUsers();
                    StaticContainer.ShowNotification("User Removed", $"User {obj.DisplayName} is deactivated on {DateTime.Now.ToString("yyyy/MM/dd")}", NotificationType.Success);
                }
            }
        }

        private void EditUser(UserWrapper obj)
        {
            UserBO userBO = new UserBO();
            this.NewUser.Id = obj.Id;
            this.NewUser.DisplayName = obj.DisplayName;
            this.NewUser.UserName = obj.UserName;
            this.NewUser.Password = userBO.DecryptPassword(obj.Password).Result;
            this.NewUser.IsAdmin = obj.IsAdmin;
            this.NewUser.PromptForPasswordReset = obj.PromptForPasswordReset;
            this.PasswordTextBox.Password = this.NewUser.Password;
            this.ButtonText = "Update User";
        }

        private bool OnCreateUserCanExecute(PasswordBox txtPwdBox)
        {
            return this.NewUser != null && !this.NewUser.HasErrors && HasChanges && !string.IsNullOrEmpty(txtPwdBox.Password);
        }

        private async void OnCreateUserExecute(PasswordBox obj)
        {
            try
            {
                PasswordTextBox = obj;
                if (_isLogoChanged)
                {
                    FileUtility.SaveProfileFile(LogoFullPathName, NewUser.ProfileImage);
                }

                User u = new User
                {
                    Id = this.NewUser.Id,
                    UserName = this.NewUser.UserName,
                    DisplayName = this.NewUser.DisplayName,
                    Password = obj.Password,
                    IsAdmin = this.NewUser.IsAdmin,
                    PromptForPasswordReset = this.NewUser.PromptForPasswordReset,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    ProfileImage = this.NewUser.ProfileImage
                };

                string title = "";
                string msg = "";
                UserBO userBO = new UserBO();
                int id = 0;
                if (u.Id > 0)
                {
                    u.Password = await userBO.EncryptPassword(u.Password);
                    id = await userBO.UpdateUser(u);
                    ButtonText = "Create Account";
                    title = "User Updated";
                    msg = $"User {u.UserName} is updated successfully.";
                }
                else
                {
                    id = await userBO.SaveUser(u);
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
            UserBO userBO = new UserBO();
            List<User> _users = userBO.GetAllUser(me.UserName);
            UsersList = new ObservableCollection<UserWrapper>();
            foreach (User u in _users)
            {
                UserWrapper userWrapper = new UserWrapper(u);
                UsersList.Add(userWrapper);
            }
            _cacheService.SetCache(CacheKey.UserList.ToString(), UsersList);
        }

        private void ManageUserInCollection(User obj)
        {
            UserWrapper u = UsersList.Where(x => x.Id == obj.Id).FirstOrDefault();
            if(u != null)
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
            this.PasswordTextBox.Password = "";
        }

        

    }
}

