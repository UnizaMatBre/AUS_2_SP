using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppBackend;

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

    public MainWindowViewModel() {

        this.CurrentViewModel = new TreeSelectionViewModel(this._OnSelectContinue);
    }

    private void _OnSelectContinue(TreeChoices choice) {

        this.CurrentViewModel = new WorkingViewModel(choice);
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        
        PropertyChanged?.Invoke( 
            this, new PropertyChangedEventArgs(propertyName)
        );
    }
    
}