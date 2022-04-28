using Bank.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Bank.Views.Windows;

public partial class LoginWindow : Window
{
    private LoginWindow() => InitializeComponent();

    public static LoginWindow Instance { get; } = new();

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.Password = ((PasswordBox)sender).SecurePassword.ToString();
    }
}