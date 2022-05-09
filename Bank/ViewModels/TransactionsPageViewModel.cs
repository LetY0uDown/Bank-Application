﻿using Bank.Core.Objects;
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
        DoTransactionCommand = new(o =>
        {

            if (PhoneNumber!.Equals(App.CurrentUser!.PhoneNumber))
            {
                new WarningWindow("Ошибка перевода", "Вы не можете переводить деньги сами себе!").ShowDialog();
                PhoneNumber = string.Empty;
                return;
            }

            User reciever = DataProvider.TryGetUserByPhoneNumber(PhoneNumber!);

            if (reciever is null)
            {
                new WarningWindow("Ошибка ввода", "Данный пользователь не найден. Проверьте правильность ввода или попробуйте ввести номер в формате +0-000-000-00-00").Show();
                return;
            }

            if (string.IsNullOrEmpty(Message))
                Message = " ";

            if (!App.CurrentUser!.SendTransaction(reciever, TransactionSum, Message!))
            {
                new WarningWindow("Ошибка!", "На вашем счёте недостаточно средств для совершения перевода").Show();
                TransactionSum = decimal.Zero;
                return;
            }

            PhoneNumber = string.Empty;
            TransactionSum = decimal.Zero;
            Message = string.Empty;

        }, b => !string.IsNullOrEmpty(PhoneNumber) && TransactionSum > decimal.Zero);

        PayCommand = new(o =>
        {
            if (!App.CurrentUser!.Pay(SelectedType!, PaymentSum))
            {
                new WarningWindow("Ошибка!", "На вашем счёте недостаточно средств для оплаты").Show();
                PaymentSum = decimal.Zero;
                return;
            }

            SelectedType = string.Empty;
            AccountNumber = string.Empty;
            PaymentSum = decimal.Zero;

        }, b => !string.IsNullOrEmpty(SelectedType)
                && !string.IsNullOrEmpty(AccountNumber)
                && PaymentSum > decimal.Zero);
    }
    private const int _maxMessageLength = 150;
    private string? _message;

    public string? PhoneNumber { get; set; }
    public decimal TransactionSum { get; set; }
    public string? Message
    {
        get => _message;
        set
        {
            _message = value;
            RemainingMessageLength = _maxMessageLength - value!.Length;
        }
    }

    public int RemainingMessageLength { get; set; } = 150;

    public string? SelectedType { get; set; }
    public string? AccountNumber { get; set; }
    public decimal PaymentSum { get; set; }

    public static List<Payment> Payments => App.CurrentUser!.Payments!;

    public Command DoTransactionCommand { get; }
    public Command PayCommand { get; }
}