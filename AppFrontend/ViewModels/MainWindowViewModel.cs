using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppBackend;

namespace AppFrontend.ViewModels;

public interface INavigationInterface {
    public object CurrentViewModel { get; set; }

    public void NavigateToMainWorkingView();
}


public sealed class MainWindowViewModel : INotifyPropertyChanged, INavigationInterface {


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

        this.NavigateToMainWorkingView();
    }

    public void NavigateToMainWorkingView() {
        
        this.CurrentViewModel = new WorkingViewModel(this, this._model!);

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        
        PropertyChanged?.Invoke( 
            this, new PropertyChangedEventArgs(propertyName)
        );
    }
    
}