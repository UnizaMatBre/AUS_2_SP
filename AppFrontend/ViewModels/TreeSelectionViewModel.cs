using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppBackend;
using AppFrontend.Foundation;

namespace AppFrontend.ViewModels;



public class TreeSelectionViewModel : INotifyPropertyChanged {
    
    private readonly Action<TreeChoices> _onContinue;

    private TreeChoices? _selectedChoices;
    
    public ObservableCollection<TreeChoices> Choices { get; }

    public TreeChoices? SelectedChoice {
        get => this._selectedChoices;
        set {
            this._selectedChoices = value;
            this.OnPropertyChanged(nameof(SelectedChoice));
            ((RelayCommand)this.ContinueCommand).RaiseCanExecuteChanged();
        }
    }
    
    public ICommand ContinueCommand { get; }
    
    public TreeSelectionViewModel(Action<TreeChoices> onContinue) {
        this._onContinue = onContinue;

        this.ContinueCommand = new RelayCommand(
            () => {
                this._onContinue(SelectedChoice!.Value);
            },
            () => SelectedChoice.HasValue
        );

        this.Choices = new(Enum.GetValues<TreeChoices>());
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}