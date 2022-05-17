using Bank.Core.Objects;
using Bank.Core.Tools;
using Bank.Models;
using System.Windows;

namespace Bank.Views.Windows;

public partial class PasswordChangeWindow : Window
{
    public PasswordChangeWindow()
    {
        InitializeComponent();
        DataContext = this;

        ChangePasswordCommand = new(o =>
        {
            if (!IsPasswordsValid()) return;

            if (App.CurrentUser!.ChangePassword(Password!, NewPassword!))
            {
                WarningBox.Show("Введённый пароль не совпадает с нынешним!");
                return;
            }

        }, b => !string.IsNullOrEmpty(Password) &&
                !string.IsNullOrEmpty(NewPassword) &&
                !string.IsNullOrEmpty(NewPasswordRepeat));
    }

    public string? Password { get; private set; }
    public string? NewPassword { get; private set; }
    public string? NewPasswordRepeat { get; private set; }

    public Command ChangePasswordCommand { get; }

    private bool IsPasswordsValid()
    {
        if (!User.PasswordRegex.IsMatch(NewPassword!))
        {
            WarningBox.Show("Новый пароль введён неверно! Он должен быть от 6 до 20 символов длинной и содержать только буквы латинского алфавита и спец. знаки");
            return false;
        }

        if (!NewPassword!.Equals(NewPasswordRepeat))
        {
            WarningBox.Show("Введённые пароли не совпадают!");
            return false;
        }

        return true;
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e) => Hide();

    private void OldPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = OldPasswordBox.Password;
        NewPassword = NewPasswordBox.Password;
        NewPasswordRepeat = RepeatNewPasswordBox.Password;
    }
}