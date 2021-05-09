using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class UserViewModel: ViewModelBase
    {
        private string _buttonText = "Create Account";
        private bool _changePasswordOnFirstLogin = true;
        private int _id;
        private string _un;
        private string _dn;
        private string _pwd;
        private bool _isadmin;
        private ObservableCollection<User> _userList;


        public int Id
        {
            get { return _id; }
            set { 
                _id = value;
                OnPropertyChanged();
            }
        }
        public string UserName
        {
            get { return _un; }
            set
            {
                _un = value;
                OnPropertyChanged();
            }
        }
        public string DisplayName
        {
            get { return _dn; }
            set
            {
                _dn = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return _pwd; }
            set
            {
                _pwd = value;
                OnPropertyChanged();
            }
        }
        public bool IsAdmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                OnPropertyChanged();
            }
        }
        public bool PromptForPasswordReset { 
            get {
                return _changePasswordOnFirstLogin; 
            }
            set { 
                _changePasswordOnFirstLogin = value; 
                OnPropertyChanged(); 
            } 
        }


        public PasswordBox PasswordTextBox { get; set; }

        

        public ObservableCollection<User> UsersList
        {
            get { return _userList; }
            set { _userList = value;
                OnPropertyChanged();
            }
        }

        

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; OnPropertyChanged(); }
        }



        private MetroWindow  _window { get; set; }

        public ICommand CreateUserCommand { get;  }
        public ICommand EditUserCommand { get;  }
        public ICommand DeleteUserCommand { get;  }
        public ICommand ResetUserCommand { get;  }


        public UserViewModel()
        {
            this._window = Application.Current.MainWindow as MetroWindow;
            CreateUserCommand = new DelegateCommand<PasswordBox>(CreateUser);
            EditUserCommand = new DelegateCommand<User>(EditUser);
            DeleteUserCommand = new DelegateCommand<User>(DeleteUser);
            ResetUserCommand = new DelegateCommand<PasswordBox>(ResetUser);

            LoadAllUsers();
        }

        private void ResetUser(PasswordBox obj)
        {
            this.PasswordTextBox = obj;
            ClearAll();
        }

        private async void DeleteUser(User obj)
        {
            if(obj != null && obj.Id>0)
            {
                obj.DeactivationDate = DateTime.Now;
                obj.PromptForPasswordReset = false;
                obj.IsActive = false;
                UserBO userBO = new UserBO();
                int i = await userBO.UpdateUser(obj);
                if(i>0)
                {
                    LoadAllUsers();
                    await _window.ShowMessageAsync("User Removed", $"User {obj.DisplayName} is deactivated on {DateTime.Now.ToString("yyyy/MM/dd")}");
                }
            }
        }

        private void EditUser(User obj)
        {
            UserBO userBO = new UserBO();

            this.Id = obj.Id;
            this.DisplayName = obj.DisplayName;
            this.UserName = obj.UserName;
            this.Password = userBO.DecryptPassword(obj.Password).Result;
            this.IsAdmin = obj.IsAdmin;
            this.PromptForPasswordReset = obj.PromptForPasswordReset;
            this.PasswordTextBox.Password = this.Password;
            this.ButtonText = "Update User";
        }

        private async void CreateUser(PasswordBox obj)
        {
            PasswordTextBox = obj;
            User u = new User
            {
                Id = this.Id,
                UserName = this.UserName,
                DisplayName = this.DisplayName,
                Password = obj.Password,
                IsAdmin = this.IsAdmin,
                PromptForPasswordReset = this.PromptForPasswordReset,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            string title="";
            string msg ="";
            UserBO userBO = new UserBO();
            int id = 0;
            if (u.Id>0)
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
               await _window.ShowMessageAsync(title, msg, MessageDialogStyle.Affirmative, StaticContainer.DialogSettings);
            }

        }

        private void LoadAllUsers()
        {
            UserBO userBO = new UserBO();
            List<User> _user = userBO.GetAllUser();
            UsersList = new ObservableCollection<User>(_user);
        }

        private void ClearAll()
        {
            this.ButtonText = "Create Account";
            this.Id = 0;
            this.UserName = "";
            this.Password = "";
            this.DisplayName = "";
            this.IsAdmin = false;
            this.PromptForPasswordReset = true;
            this.PasswordTextBox.Password = "";
        }
    }
}

