using Bank.Core.Objects;
using Bank.Core.Tools;
using Bank.Models;
using Bank.Properties;
using Bank.Views.Windows;
using System.Windows;

namespace Bank.ViewModels;

public sealed class RegistrationWindowViewModel
{
    public RegistrationWindowViewModel()
    {
        CreateAccountCommand = new(o =>
        {
            if (!IsPropertiesValid()) return;

            bool isUserExist = DataProvider.TryGetUserByPhoneNumber(PhoneNumber!) is not null;

            if (isUserExist)
            {
                WarningBox.Show("Ошибка регистрации", "Пользователь с таким номером телефона уже существует!");
                return;
            }

            User user = new()
            {
                PhoneNumber = PhoneNumber!,
                FirstName = FirstName!,
                Surname = Surname!,
                LastName = LastName!,
                Birthday = Birthday!,
                Password = Password!
            };

            DataProvider.Insert(user);
            user.InitPayments(true);

            App.CurrentUser = user;

            SaveUserData();

            RegistrationWindow.Instance.Hide();
            App.Start();

        }, b => !string.IsNullOrEmpty(PhoneNumber) &&
                !string.IsNullOrEmpty(FirstName) &&
                !string.IsNullOrEmpty(Surname) && !string.IsNullOrEmpty(LastName) &&
                !string.IsNullOrEmpty(Birthday) &&
                !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(SecondPassword));
    }

    public string? PhoneNumber { get; set; }

    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? LastName { get; set; }

    public string? Birthday { get; set; }

    public static string? Password { get; set; }
    public static string? SecondPassword { get; set; }

    public Command? CreateAccountCommand { get; }
    public Command GoBackCommand { get; } = new(o =>
    {
        RegistrationWindow.Instance.Hide();
        Application.Current.MainWindow.Show();
    });

    public bool IsPropertiesValid()
    {
        if (!User.BirthdayRegex.IsMatch(Birthday!))
        {
            WarningBox.Show("Ошибка формата ввода даты", "Дата введена в неверном формате. Попробуйте ввести её в одном из приведённых ниже форматов:\nДД.ММ.ГГГГ\nДД-ММ-ГГГГ\nДД/ММ/ГГГГ");
            return false;
        }

        if (!User.PhoneNumberRegex.IsMatch(PhoneNumber!))
        {
            WarningBox.Show("Ошибка формата ввода номера телефона", "Номер телефона, вероятно, введён неверно. Попобуйте ввести в подобном формате:\t+0-000-000-00-00");
            return false;
        }

        if (!User.PasswordRegex.IsMatch(Password!))
        {
            WarningBox.Show("Ошибка ввода пароля", "Пароль должен быть от 6 до 20 символов длинной");
            return false;
        }

        if (!User.NameRegex.IsMatch(FirstName!) && !User.NameRegex.IsMatch(Surname!) && !User.NameRegex.IsMatch(LastName!))
        {
            WarningBox.Show("Ошибка при вводе данных", "Фамилия/Имя/Отчество должны быть написани с большой буквы, используя только символы русского алфавита");
            return false;
        }

        if (!Password!.Equals(SecondPassword))
        {
            WarningBox.Show("Ошибка при вводе паролей", "Введённые пароли не совпадают! Попробуйте ввести их заного");
            return false;
        }

        return true;
    }

    private void SaveUserData()
    {
        Settings.Default.SavedPassword = Password;
        Settings.Default.SavedPhoneNumber = PhoneNumber;
        Settings.Default.Save();
    }
}