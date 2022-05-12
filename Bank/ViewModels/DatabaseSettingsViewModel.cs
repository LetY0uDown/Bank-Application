using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Properties;
using Bank.Views.Windows;
using System.Windows;

namespace Bank.ViewModels;

public sealed class DatabaseSettingsViewModel : ObservableObject
{
    public DatabaseSettingsViewModel()
    {
         SaveSettingsCommand = new(o =>
         {
             Settings.Default.DataBase = DatabaseName;
             Settings.Default.DBUser = DatabaseUser;
             Settings.Default.DBPassword = DatabasePassword;
             Settings.Default.DBServer = DatabaseServer;

             Settings.Default.Save();
             Application.Current.MainWindow.ShowDialog();

         }, b => !string.IsNullOrEmpty(DatabaseName) &&
                 !string.IsNullOrEmpty(DatabaseUser) &&
                 !string.IsNullOrEmpty(DatabasePassword) &&
                 !string.IsNullOrEmpty(DatabaseServer));
    }

    public string DatabaseName { get; set; } = Settings.Default.DataBase;

    public string DatabaseUser { get; set; } = Settings.Default.DBUser;

    public string DatabasePassword { get; set; } = Settings.Default.DBPassword;

    public string DatabaseServer { get; set; } = Settings.Default.DBServer;

    public Command SaveSettingsCommand { get; }
}