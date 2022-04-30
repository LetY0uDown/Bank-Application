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
    }

    public User CurrentUser => App.CurrentUser!;

    public Command ShowSettingsCommand { get; }

    public Page? CurrentPage { get; set; }
}