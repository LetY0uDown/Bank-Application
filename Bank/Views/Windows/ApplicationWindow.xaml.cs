using Bank.Properties;
using System.Windows;
using System.Windows.Input;

namespace Bank.Views.Windows;

public partial class ApplicationWindow : Window
{
    public ApplicationWindow() => InitializeComponent();

    private void ExitButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();    

    private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (Settings.Default.IsWindowDraggable)
            DragMove();
    }
}