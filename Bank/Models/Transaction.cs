using Bank.Core.Objects.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("transactions")]
public class Transaction : Entity
{
    public User? Sender { get; init; }
    public User? Reciever { get; init; }

    [Column(nameof(SenderID))]
    public Guid SenderID { get; init; }

    [Column(nameof(RecieverID))]
    public Guid RecieverID { get; init; }

    [Column(nameof(Sum))]
    public decimal Sum { get; init; }

    [Column(nameof(Message))]
    public string? Message { get; init; }
}