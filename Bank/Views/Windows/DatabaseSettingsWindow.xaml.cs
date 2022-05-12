using System.Windows;

namespace Bank.Views.Windows;

public partial class DatabaseSettingsWindow : Window
{
    public DatabaseSettingsWindow() => InitializeComponent();

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Hide();
        Application.Current.MainWindow.ShowDialog();
    }
}