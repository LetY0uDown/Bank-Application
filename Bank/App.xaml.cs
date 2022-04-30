using Bank.Models;
using Bank.Views.Windows;
using System.Windows;

namespace Bank;

public partial class App : Application
{
    public static User? CurrentUser { get; set; } = new() // Это для тестов, потом уберу
    {
        PhoneNumber = "+0-000-000-00-00",
        FirstName = "Имя",
        Surname = "Фамилия",
        LastName = "Отчество",
        Birthday = System.DateTime.Now,
        Balance = 12954M
    };

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