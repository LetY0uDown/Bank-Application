using System;
using System.Collections.Generic;
using Bank.Properties;

namespace Bank.Core.Controllers;

public sealed record class Theme(Uri Path, string Title);

public static class ThemeController
{
    public static Dictionary<string, Theme> Themes { get; } = new()
    {
        ["Dark Blue"] = new(Path: new("pack://application:,,,/Resources/Themes/DarkBlueTheme.xaml", UriKind.RelativeOrAbsolute),
                            Title: "Dark Blue"),

        ["Light Green"] = new(Path: new("pack://application:,,,/Resources/Themes/LightGreenTheme.xaml", UriKind.RelativeOrAbsolute),
                              Title: "Light Green")
    };

    public static Theme? CurrentTheme { get; private set; }

    public static void SetTheme(Theme theme)
    {
        CurrentTheme = theme;

        App.ThemesDictionary.Source = Themes[theme.Title].Path;

        Settings.Default.SavedTheme = theme.Title;
        Settings.Default.Save();
    }
}