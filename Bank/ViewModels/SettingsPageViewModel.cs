using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Views.Windows;
using System.Text.RegularExpressions;

namespace Bank.ViewModels;

public class SettingsPageViewModel : ObservableObject
{
    public SettingsPageViewModel()
    {
        SaveAccountCommand = new(o =>
        {
            if (!ValidateProperties()) return;

            App.CurrentUser.Birthday = System.DateTime.Parse(Birthday);

            new WarningWindow("Успешно!", "Данные сохранены").Show();

        }, b => !string.IsNullOrEmpty(App.CurrentUser!.PhoneNumber)
                && !string.IsNullOrEmpty(App.CurrentUser!.FirstName)
                && !string.IsNullOrEmpty(App.CurrentUser!.Surname)
                && !string.IsNullOrEmpty(App.CurrentUser!.LastName)
                && !string.IsNullOrEmpty(Birthday));
    }

    private readonly Regex _birthdayRegex = new(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
    private readonly Regex _phoneRegex = new(@"^\+?[0-9]-?[0-9]{3}-?[0-9]{3}-?[0-9]{2}-?[0-9]{2}$");

    public string Birthday { get; set; } = App.CurrentUser!.Birthday.ToShortDateString();

    public Command? SaveAccountCommand { get; }

    public bool ValidateProperties()
    {
        if (!_birthdayRegex.IsMatch(Birthday))
        {
            new WarningWindow("Ошибка ввода даты", "Дата введена в неверном формате. Попробуйте ввести её в одном из приведённых ниже форматов:\nДД.ММ.ГГГГ\nДД-ММ-ГГГГ\nДД/ММ/ГГГГ").Show();
            return false;
        }

        if (!_phoneRegex.IsMatch(App.CurrentUser.PhoneNumber!))
        {
            new WarningWindow("Ошибка формата ввода номера телефона", "Номер телефона, вероятно, введён неверно. Попобуйте ввести в подобном формате:\t+0-000-000-00-00").Show();
            return false;
        }

        return true;
    }
}