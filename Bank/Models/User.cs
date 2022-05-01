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

    public List<Payment> Payments { get; } = new();

    [NotMapped]
    public List<Transaction> RecievedTransactions { get; } = new();
    [NotMapped]
    public List<Transaction> SendedTransactions { get; } = new();

    public override string ToString() => $"{Surname} {FirstName[0]}. {LastName[0]}.";

    public bool SendTransaction(User reciever, decimal sum, string message)
    {
        Transaction transaction = new()
        {
            Sender = this,
            Reciever = reciever,
            Sum = sum,
            Message = message
        };

        if (Balance - sum < decimal.Zero)
            return false;

        Balance -= sum;
        WastedMoney += sum;
        SendedTransactions.Add(transaction);
        Payments.Add(new(sum, PaymentType.Transactions));

        reciever.RecieveTransaction(transaction);

        DataProvider.AddEntity<Transaction>(transaction);

        return true;
    }

    public void RecieveTransaction(Transaction transaction)
    {
        Balance += transaction.Sum;
        RecievedMoney += transaction.Sum;
        RecievedTransactions.Add(transaction);
    }
}