/*! MoonPdf - A WPF-based PDF Viewer application that uses the MoonPdfLib library
Copyright (C) 2013  (see AUTHORS file)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
!*/
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;

namespace MoonPdf
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
