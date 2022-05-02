using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("User")]
public sealed class User : Entity
{
    public string PhoneNumber { get; set; }
    public string Password { get; init; }

    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string LastName { get; set; }

    public DateTime Birthday { get; set; }

    public decimal Balance { get; private set; }

    public decimal RecievedMoney { get; private set; }
    public decimal WastedMoney { get; private set; }

    public bool IsBanned { get; set; }

    public List<Transaction> Transactions { get; } = new();

    public List<Payment> Payments { get; } = new()
    {
        new ("Переводы"),
        new ("Животные"),
        new ("Транспорт"),
        new ("Ком. Услуги"),
        new ("Медицина"),
        new ("Образование"),
        new ("Одежда"),
        new ("Отдых"),
        new ("Техника"),
        new ("Еда")
    };

    [NotMapped]
    public List<Transaction> RecievedTransactions { get; } = new();
    [NotMapped]
    public List<Transaction> SendedTransactions { get; } = new();

    public override string ToString() => $"{Surname} {FirstName[0]}. {LastName[0]}.";

    public bool Pay(string type, decimal sum)
    {
        if (Balance - sum < 0)
            return false;

        int index = Payment.PaymentTypes.IndexOf(type);
        Payments[index].Sum += sum;
        WastedMoney += sum;

        DataProvider.UpdateUser(this);

        return true;
    }

    public bool SendTransaction(User reciever, decimal sum, string message)
    {
        if (Balance - sum < decimal.Zero)
            return false;

        Transaction transaction = new()
        {
            Sender = this,
            Reciever = reciever,
            Sum = sum,
            Message = message
        };

        Balance -= sum;
        WastedMoney += sum;
        Payments[0].Sum += sum;
        Transactions.Add(transaction);
        SendedTransactions.Add(transaction);

        reciever.RecieveTransaction(transaction);

        DataProvider.UpdateUser(this);        

        return true;
    }

    public void RecieveTransaction(Transaction transaction)
    {
        Balance += transaction.Sum;
        RecievedMoney += transaction.Sum;
        RecievedTransactions.Add(transaction);
        Transactions.Add(transaction);

        DataProvider.UpdateUser(this);
    }
}