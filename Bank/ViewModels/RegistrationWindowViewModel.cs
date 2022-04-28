using Bank.Core.Objects;
using Bank.Views.Windows;
using Bank.Models;
using Bank.Core.Tools;
using System.Windows;
using System;
using System.Text.RegularExpressions;

namespace Bank.ViewModels;

public sealed class RegistrationWindowViewModel
{
    public RegistrationWindowViewModel()
    {
        CreateAccountCommand = new(o =>
        {
            if (!ValidateProperties()) return;

            User user = new()
            {
                PhoneNumber = PhoneNumber!,
                FirstName = FirstName!,
                Surname = Surname!,
                LastName = LastName!,
                Birthday = DateOnly.Parse(Birthday!),
                Password = Password!
            };

            //DataProvider.AddUser(user);

            App.CurrentUser = user;

            RegistrationWindow.Instance.Hide();
            App.Start();

        }, b => !string.IsNullOrEmpty(PhoneNumber) &&
                !string.IsNullOrEmpty(FirstName) &&
                !string.IsNullOrEmpty(Surname) && !string.IsNullOrEmpty(LastName) &&
                !string.IsNullOrEmpty(Birthday) &&
                !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(SecondPassword));
    }
    private readonly Regex _birthdayRegex = new(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
    private readonly Regex _phoneRegex = new(@"^\+?[0-9]-?[0-9]{3}-?[0-9]{3}-?[0-9]{2}-?[0-9]{2}$");

    public string? PhoneNumber { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? LastName { get; set; }

    public string? Birthday { get; set; } = string.Empty;

    public static string? Password { get; set; } = string.Empty;
    public static string? SecondPassword { get; set; } = string.Empty;

    public Command? CreateAccountCommand { get; }

    public bool ValidateProperties()
    {
        if (!_birthdayRegex.IsMatch(Birthday!))
        {
            new WarningWindow("Ошибка ввода даты","Дата введена в неверном формате. Попробуйте ввести её в одном из приведённых ниже форматов:\nДД.ММ.ГГГГ\nДД-ММ-ГГГГ\nДД/ММ/ГГГГ").Show();
            return false;
        }

        if (!_phoneRegex.IsMatch(PhoneNumber!))
        {
            new WarningWindow("Ошибка формата ввода номера телефона", "Номер телефона, вероятно, введён неверно. Попобуйте ввести в подобном формате:\t+0-000-000-00-00").Show();
            return false;
        }

        if (!Password!.Equals(SecondPassword))
        {
            new WarningWindow("Ошибка при вводе паролей", "Введённые пароли не совпадают! Попробуйте ввести их заного").Show();
            return false;
        }

        return true;
    }
}