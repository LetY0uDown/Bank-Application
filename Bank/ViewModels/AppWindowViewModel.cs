using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Views.Pages;
using System.Windows.Controls;

namespace Bank.ViewModels;

public sealed class AppWindowViewModel : ObservableObject
{
    public AppWindowViewModel()
    {
        ShowSettingsCommand = new(o =>
            CurrentPage = new SettingsPage());

        ShowCurrencyExchangeCommand = new(o =>
            CurrentPage = new CurrencyExchangePage());

        ShowTransactionsCommand = new(o =>
            CurrentPage = new TransactionsPage());

        ShowExpensesCommand = new(o =>
            CurrentPage = new ExpensesPage());
    }

    public Command ShowSettingsCommand { get; }
    public Command ShowCurrencyExchangeCommand { get; }
    public Command ShowTransactionsCommand { get; }
    public Command ShowExpensesCommand { get; }

    public Page? CurrentPage { get; set; }
}