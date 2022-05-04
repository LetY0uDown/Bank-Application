﻿using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using System.Windows;
using Bank.Views.Windows;
using Bank.Properties;

namespace Bank.ViewModels;

public sealed class LoginWindowViewModel : ObservableObject
{
    public LoginWindowViewModel()
    {
        LoginCommand = new(o =>
        {
            var user = DataProvider.TryGetUserByPhoneNumber(PhoneNumber!);

            if (user is not null && Password!.Equals(user.Password))
            {
                user.IsBanned = user.WastedMoney > user.RecievedMoney;

                if (user.IsBanned)
                {
                    new WarningWindow("Ошибка доступа", "К сожалению, ваш аккаунт заблокирован из-за подозрений в мошенничестве. Но вы можете создать новый").Show();
                    return;
                }

                App.CurrentUser = user;
            }
            else
            {
                new WarningWindow("Ошибка входа", "Неверный номер телефона и(или) пароль. Попробуйте ввести их ещё раз").Show();
                return;
            }

            if (RememberUser)
                SaveUserData();
            else
                DeleteUserData();

            LoginWindow.Instance.Hide();
            App.Start();

        }, b => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PhoneNumber));

        CreateAccountCommand = new(o =>
        {
            if (RememberUser)
                SaveUserData();
            else
                DeleteUserData();

            LoginWindow.Instance.Hide();
            RegistrationWindow.Instance.Show();
        });
    }

    public string? PhoneNumber { get; set; } = Settings.Default.SavedPhoneNumber;
    public static string? Password { get; set; } = Settings.Default.SavedPassword;

    public static bool RememberUser { get; set; } = true;

    public Command ExitCommand { get; } = new(o =>
        Application.Current.Shutdown());

    public Command MinimizeCommand { get; } = new(o =>
        LoginWindow.Instance.WindowState = WindowState.Minimized);

    public Command LoginCommand { get; } 

    public Command CreateAccountCommand { get; } 

    private void DeleteUserData()
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