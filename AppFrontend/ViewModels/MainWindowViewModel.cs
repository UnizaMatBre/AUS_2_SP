using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppBackend;

namespace AppFrontend.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged {


    private Model? _model;
    
    private object _currentViewModel;
    public object CurrentViewModel {
        
        get => _currentViewModel;
        set {
            this._currentViewModel = value;
            this.OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    private string _statusDisplay;

    public string StatusDisplay {
        get => _statusDisplay;
        set {
            this._statusDisplay = value;
            this.OnPropertyChanged(nameof(StatusDisplay));
        }
    }

    public MainWindowViewModel() {
        this._model = null;
        this.CurrentViewModel = new TreeSelectionViewModel(this._OnSelectContinue);
        this.StatusDisplay = ".......";
    }

    private void _OnSelectContinue(TreeChoices choice) {

        this._model = new Model(choice);
    
        this.GoToMainWorkView();
    }


    public void GoToMainWorkView() {
        this.CurrentViewModel = new WorkingViewModel(this._model);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        
        PropertyChanged?.Invoke( 
            this, new PropertyChangedEventArgs(propertyName)
        );
    }
    
}