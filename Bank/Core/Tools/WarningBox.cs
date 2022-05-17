using Bank.Views.Windows;

namespace Bank.Core.Tools;

public static class WarningBox
{
    public static void Show(string title, string message) =>
        new WarningWindow(title, message).ShowDialog();

    public static void Show(string message) =>
        new WarningWindow(string.Empty, message).ShowDialog();
}