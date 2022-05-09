using Bank.Core.Objects.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("transactions")]
public class Transaction : Entity
{
    public User? Sender { get; set; }
    public User? Reciever { get; set; }

    [Column(nameof(SenderID))]
    public Guid SenderID { get; set; }

    [Column(nameof(RecieverID))]
    public Guid RecieverID { get; set; }

    [Column(nameof(Sum))]
    public decimal Sum { get; set; }

    [Column(nameof(Message))]
    public string? Message { get; set; }
}