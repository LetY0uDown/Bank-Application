using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using Bank.Views.Windows;
using Bank.Models;
using System.Collections.Generic;

namespace Bank.ViewModels;

public sealed class TransactionsPageViewModel : ObservableObject
{
    public TransactionsPageViewModel()
    {
        ShowRecievedTransactionCommand = new(o =>
            TransactionsSource = App.CurrentUser!.RecievedTransactions);

        ShowSendedTransactionCommand = new(o =>
            TransactionsSource = App.CurrentUser!.SendedTransactions);

        DoTransactionCommand = new(o =>
        {
            Message = string.Empty;
           // User reciever = DataProvider.TryGetUserByPhoneNumber(PhoneNumber!);

           // if (reciever is null)
           // {
           //     new WarningWindow("Ошибка ввода", "Данный пользователь не найден. Проверьте правильность ввода или попробуйте ввести номер в формате +0-000-000-00-00").Show();
           //     return;
           // }

           //if (!App.CurrentUser.SendTransaction(reciever, Sum, Message!))
           // {
           //     new WarningWindow("Ошибка!", "На вашем счёте недостаточно средств для совершения перевода").Show();
           //     return;
           // }

        }, b => !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(Sum.ToString()));
    }

    public string PhoneNumber { get; set; }
    public decimal Sum { get; set; }
    public string Message { get; set; }

    public List<Transaction> TransactionsSource { get; set; } = App.CurrentUser!.SendedTransactions;

    public Command DoTransactionCommand { get; }

    public Command ShowSendedTransactionCommand { get; }
    public Command ShowRecievedTransactionCommand { get; }
}