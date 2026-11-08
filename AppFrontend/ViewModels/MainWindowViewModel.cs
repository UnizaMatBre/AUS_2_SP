using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppBackend;

namespace AppFrontend.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged {
    private object _currentViewModel;

    private Model? _model;
    
    public object CurrentViewModel {
        
        get => _currentViewModel;
        set {
            this._currentViewModel = value;
            this.OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public MainWindowViewModel() {
        this._model = null;
        this.CurrentViewModel = new TreeSelectionViewModel(this._OnSelectContinue);
    }

    private void _OnSelectContinue(TreeChoices choice) {

        this._model = new Model(choice);
        
        this.CurrentViewModel = new WorkingViewModel(this._model);
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        
        PropertyChanged?.Invoke( 
            this, new PropertyChangedEventArgs(propertyName)
        );
    }
    
}