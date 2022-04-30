using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using System.Windows;
using Bank.Views.Windows;

namespace Bank.ViewModels;

public sealed class LoginWindowViewModel : ObservableObject
{
    public LoginWindowViewModel()
    {
        LoginCommand = new(o =>
        {
            var user = DataProvider.TryGetUserByPhoneNumber(PhoneNumber!);

            if (user is not null && Password!.Equals(user.Password))
                App.CurrentUser = user;
            else
            {
                new WarningWindow("Ошибка входа", "Неверный номер телефона и(или) пароль. Попробуйте ввести их ещё раз").Show();
                return;
            }

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

    public Command LoginCommand { get; } 

    public Command CreateAccountCommand { get; } = new(o =>
    {
        LoginWindow.Instance.Hide();
        RegistrationWindow.Instance.Show();
    });
}