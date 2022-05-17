using Bank.Core.Controllers;
using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using Bank.Models;
using Bank.Properties;
using Bank.Views.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Bank.ViewModels;

public sealed class SettingsPageViewModel : ObservableObject
{
    public SettingsPageViewModel()
    {
        SaveAccountCommand = new(o =>
        {
            if (!IsPropertiesValid()) return;

            App.CurrentUser.Birthday = Birthday;
            DataProvider.Update(App.CurrentUser);

            App.CurrentUser.SetTransactions(DataProvider.GetTransactions(App.CurrentUser.ID));

            WarningBox.Show("Успешно!", "Данные вашего аккаунта успешно сохранены");

        }, b => !string.IsNullOrEmpty(App.CurrentUser?.PhoneNumber)
                && !string.IsNullOrEmpty(App.CurrentUser?.FirstName)
                && !string.IsNullOrEmpty(App.CurrentUser?.Surname)
                && !string.IsNullOrEmpty(App.CurrentUser?.LastName)
                && !string.IsNullOrEmpty(Birthday));

        DepositMoneyCommand = new(o => DepositMoney(500));
    }

    public static bool IsDraggable
    {
        get => Settings.Default.IsWindowDraggable;
        set
        {
            Settings.Default.IsWindowDraggable = value;
            Settings.Default.Save();
        }
    }

    public string Birthday { get; set; } = App.CurrentUser!.Birthday!;
    public decimal Balance { get; set; } = App.CurrentUser!.Balance;

    public List<Theme> Themes { get; } = ThemeController.Themes.Values.ToList();
    public static Theme SelectedTheme
    {
        get => ThemeController.CurrentTheme!;
        set => ThemeController.SetTheme(value);
    }

    public Command SaveAccountCommand { get; }
    public Command DeleteAccountCommand { get; } = new(o =>
    {
        Application.Current.MainWindow.Hide();

        Settings.Default.SavedPhoneNumber = string.Empty;
        Settings.Default.SavedPassword = string.Empty;
        Settings.Default.Save();

        foreach (var p in App.CurrentUser.Payments!)
            DataProvider.Delete(p);

        DataProvider.Delete(App.CurrentUser);
        App.CurrentUser = null;

        App.ShowLoginWindow();
    });

    public Command ShowPasswordChangeCommand { get; } = new(o => new PasswordChangeWindow().ShowDialog());

    public Command DepositMoneyCommand { get; }

    private bool IsPropertiesValid()
    {
        if (!User.BirthdayRegex.IsMatch(Birthday))
        {
            WarningBox.Show("Ошибка ввода даты", "Дата введена в неверном формате. Попробуйте ввести её в одном из приведённых ниже форматов:\nДД.ММ.ГГГГ\nДД-ММ-ГГГГ\nДД/ММ/ГГГГ");
            return false;
        }

        if (!User.PhoneNumberRegex.IsMatch(App.CurrentUser!.PhoneNumber!))
        {
            WarningBox.Show("Ошибка формата ввода номера телефона", "Номер телефона, вероятно, введён неверно. Попобуйте ввести в подобном формате:\t+0-000-000-00-00");
            return false;
        }

        if (!User.NameRegex.IsMatch(App.CurrentUser.FirstName!) &&
            !User.NameRegex.IsMatch(App.CurrentUser.Surname!) &&
            !User.NameRegex.IsMatch(App.CurrentUser.LastName!))
        {
            WarningBox.Show("Ошибка ввода данных", "Фамилия/Имя/Отчество должны быть написани с большой буквы, используя только символы русского алфавита");
            return false;
        }

        return true;
    }
    private void DepositMoney(decimal sum)
    {
        Balance += sum;
        App.CurrentUser!.DepositMoney(sum);
    }
}
