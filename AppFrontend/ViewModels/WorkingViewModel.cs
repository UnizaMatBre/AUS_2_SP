using System.Collections.ObjectModel;
using System.Windows.Input;
using AppBackend;
using AppFrontend.Foundation;
using AppFrontend.ViewModels.Components;

namespace AppFrontend.ViewModels;

public class WorkingViewModel {
    private readonly Model _model;

    public ObservableCollection<OperationDescriptor> Operations { get; } = new();
    public ObservableCollection<InputFieldViewModel> InputFields { get; } = new();
    
    public ICommand SubmitCommand { get; }

    
    
    private int _selectedOperationIndex = 0;
    public int SelectedOperationIndex {
        get => this._selectedOperationIndex;
        set {
            this._selectedOperationIndex = value;
            this._UpdateInputFields();
        }
    }
    
    public WorkingViewModel(Model model) {
        this._model = model;
        this._InitializeOperations();
        this.SubmitCommand = new RelayCommand(this._Submit, () => true);

        this.SelectedOperationIndex = 0;
    }

    private void _InitializeOperations() {
        
    }
    
    private void _UpdateInputFields() {
        this.InputFields.Clear();
        var descriptor = this.Operations[this.SelectedOperationIndex];
        foreach (var inputName in descriptor.InputNames) {
            this.InputFields.Add(
                new InputFieldViewModel(inputName)
            );
        }
    }

    private void _Submit() {
        try {
            var descriptor = this.Operations[this._selectedOperationIndex];
            
            // TODO: Implement proper operation execution

        }
        catch (Exception exc) {
            // TODO: Implement proper error display
            throw;
        }
    }
}