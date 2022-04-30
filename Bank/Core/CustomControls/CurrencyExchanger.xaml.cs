using Bank.Core.Tools;
using System.ComponentModel;
using System.Windows.Controls;

namespace Bank.Core.CustomControls;

public partial class CurrencyExchangerControl : UserControl, INotifyPropertyChanged
{
    public CurrencyExchangerControl()
    {
        InitializeComponent();
        DataContext = this;        
    }
    private CurrencyExchanger? exchanger;

    public string? FirstCurrency { get; set; }
    public string? SecondCurrency { get; set; }

    private decimal _exchangableCurrencyAmount = 1;
    public decimal ExchangableCurrencyAmount
    {
        get => _exchangableCurrencyAmount;
        set
        {
            exchanger ??= new(FirstCurrency!, SecondCurrency!);

            _exchangableCurrencyAmount = value;
            SecondCurrencyAmount = exchanger!.Exchange(ExchangableCurrencyAmount).ToString("0.###");
        }
    }
    public string? SecondCurrencyAmount { get; set; } 

    public event PropertyChangedEventHandler? PropertyChanged;
}