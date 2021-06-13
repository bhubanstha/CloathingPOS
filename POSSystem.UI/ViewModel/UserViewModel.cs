using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private MetroWindow _window { get; set; }
        private bool _hasChanges;
        private string _buttonText = "Create Account";
        private ObservableCollection<User> _userList;
        private CreateUserWrapper _newUser;
        public PasswordBox PasswordTextBox { get; set; }
        private UserBO _userBo;

        public ICommand CreateUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ResetUserCommand { get; }

        public CreateUserWrapper NewUser
        {
            get { return _newUser; }
            set
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> UsersList
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


        public UserViewModel()
        {
            this._window = Application.Current.MainWindow as MetroWindow;
            _userBo = new UserBO();
            NewUser = new CreateUserWrapper(new User());
            NewUser.PromptForPasswordReset = true;

            NewUser.PropertyChanged += NewUser_PropertyChanged;
            CreateUserCommand = new DelegateCommand<PasswordBox>(OnCreateUserExecute).ObservesProperty(() => HasChanges);
            EditUserCommand = new DelegateCommand<User>(EditUser);
            DeleteUserCommand = new DelegateCommand<User>(DeleteUser);
            ResetUserCommand = new DelegateCommand<PasswordBox>(ResetUser);

            LoadAllUsers();
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

        private async void DeleteUser(User obj)
        {
            if (obj != null && obj.Id > 0)
            {
                obj.DeactivationDate = DateTime.Now;
                obj.PromptForPasswordReset = false;
                obj.IsActive = false;
                UserBO userBO = new UserBO();
                int i = await userBO.UpdateUser(obj);
                if (i > 0)
                {
                    LoadAllUsers();
                    await _window.ShowMessageAsync("User Removed", $"User {obj.DisplayName} is deactivated on {DateTime.Now.ToString("yyyy/MM/dd")}");
                }
            }
        }

        private void EditUser(User obj)
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
                User u = new User
                {
                    Id = this.NewUser.Id,
                    UserName = this.NewUser.UserName,
                    DisplayName = this.NewUser.DisplayName,
                    Password = obj.Password,
                    IsAdmin = this.NewUser.IsAdmin,
                    PromptForPasswordReset = this.NewUser.PromptForPasswordReset,
                    IsActive = true,
                    CreatedDate = DateTime.Now
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
            UserBO userBO = new UserBO();
            List<User> _users = userBO.GetAllUser();
            UsersList = new ObservableCollection<User>();
            foreach (User u in _users)
            {
                UsersList.Add(u);
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

