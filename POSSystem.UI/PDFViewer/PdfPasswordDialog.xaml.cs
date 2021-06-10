using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.PDFViewer
{
    public partial class PdfPasswordDialog : MetroWindow
    {
        public string Password { get; private set; }

        public PdfPasswordDialog()
        {
            InitializeComponent();
            this.Loaded += PdfPasswordDialog_Loaded;
        }

        void PdfPasswordDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtPassword.Focus();
            //this.txtPassword.SelectAll();
        }

        private void BtnApplyPassword_Click(object sender, RoutedEventArgs e)
        {
            this.Password = this.txtPassword.Password;
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtPassword_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
