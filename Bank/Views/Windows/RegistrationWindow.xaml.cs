using Bank.ViewModels;
using System.Windows;

namespace Bank.Views.Windows;

public partial class RegistrationWindow : Window
{
    private RegistrationWindow() => InitializeComponent();

    public static RegistrationWindow Instance { get; } = new();

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        RegistrationWindowViewModel.Password = firstPasswordBox.Password;
    }

    private void PasswordBox_PasswordChanged_1(object sender, RoutedEventArgs e)
    {
        RegistrationWindowViewModel.SecondPassword = secondPasswordBox.Password;
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}