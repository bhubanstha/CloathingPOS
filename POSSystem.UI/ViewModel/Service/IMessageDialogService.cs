using MahApps.Metro.Controls;
using System.Windows;

namespace POSSystem.UI.ViewModel.Service
{
    public interface IMessageDialogService
    {
        MessageBoxResult ShowDialog(string text, string title, MessageBoxButton button, MessageBoxImage icon);
        void ShowDialog(string text, MetroWindow window);
    }
}