using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Core.Controllers;

public record Theme(Uri Path, string Title);

public static class ThemeController
{
    static ThemeController()
    {

    }

    private static readonly Dictionary<string, Theme> _themes = new()
    {
        ["Dark Blue"] = new(Path: new("pack://application:,,,/Resources/Themes/DarkBlueTheme.xaml", UriKind.RelativeOrAbsolute),
                            Title: "Dark Blue"),

        ["Light Green"] = new(Path: new("pack://application:,,,/Resources/Themes/LightGreenTheme.xaml", UriKind.RelativeOrAbsolute),
                              Title: "Light Green")
    };

    public static Themes CurrentTheme { get; private set; }

    public static void SetTheme(Themes theme)
    {
        CurrentTheme = theme;

        App.ThemesDictionary.Source = _themes.Values.ToList()[(int)theme].Path;
    }
}

public enum Themes
{
    DarkBlue,
    LightGreen
}