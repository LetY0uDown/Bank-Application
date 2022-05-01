using Bank.Core.Objects.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Transaction")]
public sealed class Transaction : Entity
{
    public Guid SenderID { get; set; }
    public User? Sender { get; set; }

    public Guid RecieverID { get; set; }
    public User? Reciever { get; set; }

    public decimal Sum { get; init; }

    public string? Message { get; init; }
}