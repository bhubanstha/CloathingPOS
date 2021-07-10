using MahApps.Metro.Controls;
using POSSystem.UI.Service;
using System.Threading;
using System.Windows;

namespace POSSystem.UI
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : MetroWindow
    {

        private bool _isUserNameEditable = true;
        private bool _isBackButtonVisible = true;
        private bool _lookForPasswordChange=false;
        private Timer timer = null;

        public bool IsUserNameEditable
        {
            get { return _isUserNameEditable; }
            set { _isUserNameEditable = value; }
        }

        public bool IsBackButtonVisible
        {
            get { return _isBackButtonVisible; }
            set { _isBackButtonVisible = value; }
        }

        

        public bool LookForPasswordChange
        {
            get { return _lookForPasswordChange; }
            set { _lookForPasswordChange = value; }
        }


        public ForgotPasswordWindow()
        {
            InitializeComponent();
            this.Loaded += ForgotPasswordWindow_Loaded;
        }

        private void ForgotPasswordWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vwForgetPwd.ShowBackButton = IsBackButtonVisible;
            vwForgetPwd.EnableUserName = IsUserNameEditable;
            if (LookForPasswordChange)
            {
                timer = new Timer(CheckPasswordChangeStatus, null, 3000, 500);
            }
        }

        void CheckPasswordChangeStatus(object state)
        {
            if(StaticContainer.IsPasswordChanged)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                this.Dispatcher.Invoke(() =>
                {
                    this.DialogResult = true;
                    //this.Close();
                });
                
            }
        }


    }

   
}
