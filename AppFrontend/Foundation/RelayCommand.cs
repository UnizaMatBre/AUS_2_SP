

using System.Windows.Input;

namespace AppFrontend.Foundation;


public class RelayCommand : ICommand {
    private readonly Action _action;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action action, Func<bool> canExecute) {
        this._action = action;
        this._canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) {
        return this._canExecute();
    }

    public void Execute(object? parameter) {
        this._action();
    }
    
    public event EventHandler CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
}