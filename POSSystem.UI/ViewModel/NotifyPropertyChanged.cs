using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POSSystem.UI.ViewModel
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool CompareString(string a, string b)
        {
            int i = string.Compare(a, b, System.StringComparison.OrdinalIgnoreCase);
            return i == 0;
        }
    }
}
