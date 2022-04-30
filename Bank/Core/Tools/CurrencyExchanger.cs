using System.Collections.Generic;

namespace Bank.Core.Tools;

public class CurrencyExchanger
{
    public CurrencyExchanger(string firstCurrency, string secondCurrency)
    {
        ExchangeRate = _exchangeRates[$"{firstCurrency}-{secondCurrency}"];
    }

    private static readonly Dictionary<string, decimal> _exchangeRates = new()
    {
        ["$-₽"] = 71.35M,
        ["₽-$"] = 0.014M,
        ["€-₽"] = 75.23M,
        ["₽-€"] = 0.013M, ["-"] = 15
    };

    public decimal ExchangeRate { get; set; }

    public decimal Exchange(decimal currencyAmount) => currencyAmount * ExchangeRate;
}