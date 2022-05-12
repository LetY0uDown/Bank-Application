using Bank.Properties;
using Bank.ViewModels;
using System.Windows;

namespace Bank.Views.Windows;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        if (LoginWindowViewModel.RememberUser)
            _passwordBox.Password = Settings.Default.SavedPassword.ToString();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.Password = _passwordBox.Password.ToString();
    }
}