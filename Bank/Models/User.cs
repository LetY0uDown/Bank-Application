using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Users")]
public sealed class User : Entity
{
    public string PhoneNumber { get; set; }
    public string Password { get; init; }

    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string LastName { get; set; }
    
    public DateTime Birthday { get; set; }

    [DataType("DECIMAL(65, 10)")]
    public decimal Balance { get; set; }

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
        SendedTransactions.Add(transaction);

        reciever.RecieveTransaction(transaction);

        DataProvider.AddEntity<Transaction>(transaction);

        return true;
    }

    public void RecieveTransaction(Transaction transaction)
    {
        Balance += transaction.Sum;
        RecievedTransactions.Add(transaction);
    }
}