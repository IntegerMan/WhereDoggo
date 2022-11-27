using System.Runtime.CompilerServices;

namespace MattEland.WhereDoggo.WPFClient.Helpers;

public class RelayCommand : ICommand, INotifyPropertyChanged
{
    private readonly Action _action;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action action, Func<bool>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute;
        Enabled = CanExecute(null);
    }

    public bool Enabled { get; private set; }

    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

    public void Execute(object? parameter)
    {
        _action();
        NotifyCanExecuteChanged();
    }

    public event EventHandler? CanExecuteChanged;

    public void NotifyCanExecuteChanged()
    {
        Enabled = CanExecute(null);
        CanExecuteChanged?.Invoke(null, EventArgs.Empty);
        OnPropertyChanged(nameof(Enabled));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}