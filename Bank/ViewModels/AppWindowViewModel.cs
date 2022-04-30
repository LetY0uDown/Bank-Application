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
    }

    public User CurrentUser => App.CurrentUser!;

    public Command ShowSettingsCommand { get; }
    public Command ShowCurrencyExchangeCommand { get; }

    public Page? CurrentPage { get; set; }
}