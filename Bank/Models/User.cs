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

    public decimal Balance { get; set; }

    public decimal RecievedMoney { get; private set; }
    public decimal WastedMoney { get; private set; }

    public bool IsBanned { get; set; }

    public List<Transaction> Transactions { get; } = new();

    public Payment[] Payments { get; } = new Payment[10]
    {
        new (PaymentType.Transactions),
        new (PaymentType.Animals),
        new (PaymentType.Transport),
        new (PaymentType.Utilities),
        new (PaymentType.Medicine),
        new (PaymentType.Education),
        new (PaymentType.Clothes),
        new (PaymentType.Rest),
        new (PaymentType.Technic),
        new (PaymentType.Food)
    };

    [NotMapped]
    public List<Transaction> RecievedTransactions { get; } = new();
    [NotMapped]
    public List<Transaction> SendedTransactions { get; } = new();

    public override string ToString() => $"{Surname} {FirstName[0]}. {LastName[0]}.";

    public bool Pay(PaymentType type, decimal sum)
    {
        if (Balance - sum < 0)
            return false;

        Payments[(int)type].Sum += sum;
        WastedMoney += sum;

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
        SendedTransactions.Add(transaction);        

        reciever.RecieveTransaction(transaction);

        return true;
    }

    public void RecieveTransaction(Transaction transaction)
    {
        Balance += transaction.Sum;
        RecievedMoney += transaction.Sum;
        RecievedTransactions.Add(transaction);
    }
}