using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppFrontend.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged {
    private object _currentViewModel;

    public object CurrentViewModel {
        
        get => _currentViewModel;
        set {
            this._currentViewModel = value;
            this.OnPropertyChanged(nameof(CurrentViewModel));
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        
        PropertyChanged?.Invoke( 
            this, new PropertyChangedEventArgs(propertyName)
        );
    }
    
}