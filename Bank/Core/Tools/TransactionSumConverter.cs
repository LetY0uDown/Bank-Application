using System;
using System.Globalization;
using System.Windows.Data;

namespace Bank.Core.Tools;

public sealed class TransactionSumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"Сумма: {value:0.##} ₽";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
}
