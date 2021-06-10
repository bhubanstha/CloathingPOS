using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

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
