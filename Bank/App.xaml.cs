using Bank.Models;
using Bank.Views.Windows;
using System.Windows;

namespace Bank;

public partial class App : Application
{
    public static User? CurrentUser { get; set; } = new();

    public static ResourceDictionary ThemesDictionary => Current.Resources.MergedDictionaries[0];

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        LoginWindow.Instance.ShowDialog();
    }

    public static void Start()
    {
        Current.MainWindow = new ApplicationWindow();
        Current.MainWindow.Show();
    }
}