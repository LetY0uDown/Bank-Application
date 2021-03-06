using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using System.Windows;
using Bank.Views.Windows;
using Bank.Properties;
using Bank.Core.Controllers;

namespace Bank.ViewModels;

public sealed class LoginWindowViewModel : ObservableObject
{
    public LoginWindowViewModel()
    {
        ThemeController.SetTheme(ThemeController.Themes[Settings.Default.SavedTheme]);

        LoginCommand = new(o =>
        {
            var user = DataProvider.TryGetUserByPhoneNumber(PhoneNumber!);

            if (user is null)
            {
                WarningBox.Show("Ошибка входа", "Неверный номер телефона. Попробуйте ввести его ещё раз");
                return;
            }

            if (user.IsBanned)
            {
                WarningBox.Show("Ошибка доступа", "К сожалению, ваш аккаунт заблокирован из-за подозрений в мошенничестве. Но вы можете создать новый");
                return;
            }

            if (Password!.Equals(user.Password))
            {
                user.SetTransactions(DataProvider.GetTransactions(user.ID));
                user.InitPayments(false);

                App.CurrentUser = user;
            }

            if (RememberUser)
                SaveUserData();
            else
                DeleteUserData();

            Application.Current.MainWindow.Hide();
            App.Start();

        }, b => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PhoneNumber));

        CreateAccountCommand = new(o =>
        {
            if (RememberUser)
                SaveUserData();
            else
                DeleteUserData();

            Application.Current.MainWindow.Hide();
            RegistrationWindow.Instance.Show();
        });

        ExitCommand = new(o =>
        {
            if (RememberUser)
                SaveUserData();
            else
                DeleteUserData();

            Application.Current.Shutdown();
        });
    }

    public string? PhoneNumber { get; set; } = Settings.Default.SavedPhoneNumber;
    public static string? Password { get; set; } = Settings.Default.SavedPassword;

    public static bool RememberUser { get; set; } = true;

    public Command ExitCommand { get; } 

    public Command MinimizeCommand { get; } = new(o =>
        Application.Current.MainWindow.WindowState = WindowState.Minimized);

    public Command LoginCommand { get; } 

    public Command CreateAccountCommand { get; }

    public Command ShowDatabaseSettingsCommand { get; } = new(o => 
    {
        Application.Current.MainWindow.Hide();
        new DatabaseSettingsWindow().ShowDialog(); 
    });

    private static void DeleteUserData()
    {
        Settings.Default.SavedPassword = null;
        Settings.Default.SavedPhoneNumber = null;
        Settings.Default.Save();
    }
    private void SaveUserData()
    {
        Settings.Default.SavedPassword = Password;
        Settings.Default.SavedPhoneNumber = PhoneNumber;
        Settings.Default.Save();
    }
}