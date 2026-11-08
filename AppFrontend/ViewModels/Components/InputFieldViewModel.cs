using System.ComponentModel;

namespace AppFrontend.ViewModels.Components;

public class InputFieldViewModel : INotifyPropertyChanged {
    public string Name { get; set; }

    private string _value;
    public string Value {
        get => this._value;
        set {
            this._value = value;
            this.OnPropertyChanged(nameof(Value));
        }
    }

    public InputFieldViewModel(string name) {
        this.Name = name;
        this.Value = String.Empty;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName) {
        this.PropertyChanged?.Invoke(
            this, new PropertyChangedEventArgs(propertyName)
        );
    }
}