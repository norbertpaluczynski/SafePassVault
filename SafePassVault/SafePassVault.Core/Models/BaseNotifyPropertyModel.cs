using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SafePassVault.Core.Models
{
    public class BaseNotifyPropertyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
