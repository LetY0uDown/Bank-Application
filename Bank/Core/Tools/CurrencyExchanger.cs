using System.Collections.Generic;

namespace Bank.Core.Tools;

public sealed class CurrencyExchanger
{
    public CurrencyExchanger(string firstCurrency, string secondCurrency)
    {
        ExchangeRate = _exchangeRates[$"{firstCurrency}-{secondCurrency}"];
    }

    private static readonly Dictionary<string, decimal> _exchangeRates = new()
    {
        ["$-₽"] = 66.5M,
        ["₽-$"] = 0.015M,
        ["€-₽"] = 69.2491M,
        ["₽-€"] = 0.0144M
    };

    public decimal ExchangeRate { get; set; }

    public decimal Exchange(decimal currencyAmount) => currencyAmount * ExchangeRate;
}