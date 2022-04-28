using System;
using System.Windows.Input;

namespace Bank.Core.Objects;

public sealed class Command : ICommand
{
    public Command(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) =>
        _canExecute is null || _canExecute(parameter!);

    public void Execute(object? parameter) =>
        _execute(parameter!);
}