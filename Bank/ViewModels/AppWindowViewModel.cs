using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Views.Pages;
using Bank.Models;
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

        ShowPaymentsCommand = new(o =>
            CurrentPage = new PaymentsPage());
    }

    public User CurrentUser => App.CurrentUser!;

    public static decimal Balance
    {
        get => App.CurrentUser!.Balance;
        set => App.CurrentUser!.Balance = value;
    }

    public Command ShowSettingsCommand { get; }
    public Command ShowCurrencyExchangeCommand { get; }
    public Command ShowTransactionsCommand { get; }
    public Command ShowPaymentsCommand { get; }

    public Page? CurrentPage { get; set; }
}