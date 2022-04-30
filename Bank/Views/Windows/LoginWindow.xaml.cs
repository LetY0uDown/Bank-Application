using Bank.Properties;
using Bank.ViewModels;
using System.Windows;

namespace Bank.Views.Windows;

public partial class LoginWindow : Window
{
    private LoginWindow()
    {
        InitializeComponent();

        if (LoginWindowViewModel.RememberUser)
            _passwordBox.Password = Settings.Default.SavedPassword.ToString();
    }

    public static LoginWindow Instance { get; } = new();

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.Password = _passwordBox.Password.ToString();
    }
}