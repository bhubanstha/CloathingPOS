using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace POSSystem.UI.ViewModel.Service
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageBoxResult ShowDialog(string text, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBoxResult result = MessageBox.Show(text, title, button, icon);
            return result;
        }

        public void ShowDialog(string text, MetroWindow window)
        {
            window.ShowMessageAsync(text, "");
        }
    }

}
