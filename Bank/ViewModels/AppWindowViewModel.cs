using Bank.Core.Objects.Abstract;
using Bank.Models;
using System.Windows.Controls;

namespace Bank.ViewModels;

public sealed class AppWindowViewModel : ObservableObject
{
    public User CurrentUser => App.CurrentUser!;

    public Page? CurrentPage { get; set; }
}