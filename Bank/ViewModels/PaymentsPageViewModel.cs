using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Models;
using System.Collections.Generic;

namespace Bank.ViewModels;

public sealed class PaymentsPageViewModel : ObservableObject
{
    public PaymentsPageViewModel()
    {
        PayCommand = new(o =>
        {
            //App.CurrentUser.Pay(PaymentType!, Sum);

        }, b => !string.IsNullOrEmpty(SelectedType)
                && !string.IsNullOrEmpty(AccountNumber)
                && Sum > decimal.Zero);
    }

    public decimal Sum { get; set; }
    public string SelectedType { get; set; }
    public string AccountNumber { get; set; }

    public decimal WastedMoney { get; set; } = App.CurrentUser!.WastedMoney;
    public decimal RecievedMoney { get; set; } = App.CurrentUser!.RecievedMoney;

    public List<Payment> Payments => App.CurrentUser!.Payments;

    public Command PayCommand { get; }    
}