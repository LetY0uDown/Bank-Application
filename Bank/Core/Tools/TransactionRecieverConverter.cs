using System;
using System.Globalization;
using System.Windows.Data;

namespace Bank.Core.Tools;

public sealed class TransactionRecieverConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Получил: {value}";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
}