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
            TransactionsSource = App.CurrentUser!.RecievedTransactions!);

        ShowSendedTransactionCommand = new(o =>
            TransactionsSource = App.CurrentUser!.SendedTransactions!);
    }

    public static decimal WastedMoney => App.CurrentUser!.WastedMoney;
    public static decimal RecievedMoney => App.CurrentUser!.RecievedMoney;

    public List<Transaction> TransactionsSource { get; private set; } = App.CurrentUser!.SendedTransactions!;

    public static List<Payment> Payments => App.CurrentUser!.Payments!;

    public Command ShowSendedTransactionCommand { get; }
    public Command ShowRecievedTransactionCommand { get; }
}