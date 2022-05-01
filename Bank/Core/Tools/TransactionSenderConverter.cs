using System;
using System.Globalization;
using System.Windows.Data;

namespace Bank.Core.Tools;

public sealed class TransactionSenderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Отправил: {value}";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
}