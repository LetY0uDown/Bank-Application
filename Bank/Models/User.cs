using Bank.Core.Objects.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Users")]
public sealed class User : Entity
{
    public string PhoneNumber { get; init; }
    public string Password { get; init; }

    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string LastName { get; set; }

    public DateOnly Birthday { get; set; }

    public decimal Balance { get; set; }

    public List<Transaction>? Transactions { get; set; }

    [NotMapped]
    public List<Transaction>? RecievedTransactions { get; set; }
    [NotMapped]
    public List<Transaction>? SendedTransactions { get; set; }

    public override string ToString() => $"{Surname} {FirstName[0]}. {LastName[0]}.";
}