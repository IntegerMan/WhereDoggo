using System.Windows.Input;

namespace MattEland.WhereDoggo.WPFClient.Helpers;

public class RelayCommand : ICommand
{
    private readonly Action _action;

    public RelayCommand(Action action)
    {
        _action = action;

        // This will always be null, but this code suppresses a compiler warning
        CanExecuteChanged?.Invoke(null, EventArgs.Empty);
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        _action();
    }

    public event EventHandler? CanExecuteChanged;
}