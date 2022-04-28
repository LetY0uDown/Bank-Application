using System.Windows;

namespace Bank.Views.Windows;

public partial class ApplicationWindow : Window
{
    public ApplicationWindow() => InitializeComponent();

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}