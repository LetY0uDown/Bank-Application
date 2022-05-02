using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Models;
using System.Collections.Generic;

namespace Bank.ViewModels;

public sealed class ExpensesPageViewModel : ObservableObject
{
    public ExpensesPageViewModel()
    {
        ShowRecievedTransactionCommand = new(o =>
            TransactionsSource = App.CurrentUser!.RecievedTransactions);

        ShowSendedTransactionCommand = new(o =>
            TransactionsSource = App.CurrentUser!.SendedTransactions);
    }

    public decimal WastedMoney { get; set; } = App.CurrentUser!.WastedMoney;
    public decimal RecievedMoney { get; set; } = App.CurrentUser!.RecievedMoney;

    public List<Transaction> TransactionsSource { get; set; } = App.CurrentUser!.SendedTransactions;

    public List<Payment> Payments => App.CurrentUser!.Payments;

    public Command ShowSendedTransactionCommand { get; }
    public Command ShowRecievedTransactionCommand { get; }
}