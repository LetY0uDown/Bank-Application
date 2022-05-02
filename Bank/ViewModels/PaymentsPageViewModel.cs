using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Models;

namespace Bank.ViewModels;

public sealed class PaymentsPageViewModel : ObservableObject
{
    public decimal Sum { get; set; }
    public PaymentType PaymentType { get; set; }

    public object Payments => App.CurrentUser!.Payments;

    public Command PayCommand { get; }    
}