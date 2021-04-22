using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
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
        private bool _changePasswordOnFirstLogin = true;

        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName  { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool PromptForPasswordReset { 
            get { return _changePasswordOnFirstLogin; }
            set { _changePasswordOnFirstLogin = value; } 
        }

        public MetroWindow  Window { get; set; }

        public ICommand CreateUserCommand { get;  }

        public UserViewModel()
        {
            this.Window = Application.Current.MainWindow as MetroWindow;
            CreateUserCommand = new DelegateCommand<PasswordBox>(CreateUser);
        }

        private async void CreateUser(PasswordBox obj)
        {
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

            UserBO userBO = new UserBO();
            int id = await userBO.SaveUser(u);
            if (id > 0)
            { 
                MetroDialogSettings settings = new MetroDialogSettings
                {
                    AnimateHide = true,
                    AnimateShow = true

                };
               await Window.ShowMessageAsync("User Creation", $"User {u.UserName} created successfully.", MessageDialogStyle.Affirmative, settings);
            }

        }
    }
}

