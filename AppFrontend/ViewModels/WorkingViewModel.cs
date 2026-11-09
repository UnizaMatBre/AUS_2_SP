using System.Collections.ObjectModel;
using System.Windows.Input;
using AppBackend;
using AppFrontend.Foundation;
using AppFrontend.ViewModels.Components;

namespace AppFrontend.ViewModels;

public class WorkingViewModel {
    private readonly INavigationInterface _navigation;
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
    
    public WorkingViewModel(INavigationInterface navigation, Model model) {
        this._navigation = navigation;
        this._model = model;
        this._InitializeOperations();
        this.SubmitCommand = new RelayCommand(this._Submit, () => true);

        this.SelectedOperationIndex = 0;
    }

    private void _InitializeOperations() {
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Generate Random Data",
            InputNames = [ "location_count" ]
        });
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Load Data From File",
            InputNames = []
        });

        this.Operations.Add(new OperationDescriptor {
            Name = "Dump Data Into File",
            InputNames = []
        });
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Create Location",
            InputNames = ["lat", "lon", "name", "description_optional", "address_optional"]
        });
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Find Locations in Area",
            InputNames = ["west_lon", "east_lon", "north_lat", "south_lat"]
        });

        List<string> circleSearch = ["centre_lat", "centre_lon", "radius_in_metres"];
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Find Locations in Circle",
            InputNames = circleSearch
        });
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Edit Information of Location",
            InputNames = circleSearch
        });
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Edit Coordinates of Location",
            InputNames = circleSearch
        });
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Find Location by Text In name",
            InputNames = ["centre_lat", "centre_lon", "radius_in_metres", "searched_text"]
        });
        
        this.Operations.Add(new OperationDescriptor {
            Name = "Remove Location",
            InputNames = circleSearch
        });
        
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