using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using System.Windows;
using Bank.Views.Windows;

namespace Bank.ViewModels;

public sealed class LoginWindowViewModel : ObservableObject
{
    public LoginWindowViewModel()
    {
        LoginCommand = new(o =>
        {
            LoginWindow.Instance.Hide();
            App.Start();

        }, b => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PhoneNumber));
    }

    public string? PhoneNumber { get; set; }
    public static string? Password { get; set; }

    public Command ExitCommand { get; } = new(o =>
        Application.Current.Shutdown());

    public Command MinimizeCommand { get; } = new(o =>
        LoginWindow.Instance.WindowState = WindowState.Minimized);

    public Command LoginCommand { get; init; } 

    public Command CreateAccountCommand { get; } = new(o =>
    {
        LoginWindow.Instance.Hide();
        RegistrationWindow.Instance.Show();
    });
}