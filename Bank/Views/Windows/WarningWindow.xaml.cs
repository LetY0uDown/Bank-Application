using System.Windows;

namespace Bank.Views.Windows;

public partial class WarningWindow : Window
{
    public WarningWindow(string title, string message)
    {
        InitializeComponent();
        DataContext = this;

        MessageTitle = title;
        Message = message;
    }

    public static string? MessageTitle { get; private set; }

    public static string? Message { get; private set; }

    private void ExitButton_Click(object sender, RoutedEventArgs e) => Hide();
}