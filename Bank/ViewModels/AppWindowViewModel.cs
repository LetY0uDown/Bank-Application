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
            CurrentPage = new SettingsPage(),
            b => CurrentPage is not SettingsPage);

        ShowCurrencyExchangeCommand = new(o =>
            CurrentPage = new CurrencyExchangePage(),
            b => CurrentPage is not CurrencyExchangePage);

        ShowTransactionsCommand = new(o =>
            CurrentPage = new TransactionsPage(),
            b => CurrentPage is not TransactionsPage);

        ShowExpensesCommand = new(o =>
            CurrentPage = new ExpensesPage(),
            b => CurrentPage is not ExpensesPage);
    }

    public Command ShowSettingsCommand { get; }
    public Command ShowCurrencyExchangeCommand { get; }
    public Command ShowTransactionsCommand { get; }
    public Command ShowExpensesCommand { get; }

    public Page? CurrentPage { get; set; } = new SettingsPage();
}