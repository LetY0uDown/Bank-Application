using Bank.Core.Objects.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Transactions")]
public sealed class Transaction : Entity
{
    public int SenderID { get; init; }
    public User? Sender { get; set; }

    public int RecieverID { get; init; }
    public User? Reciever { get; set; }

    public decimal Sum { get; init; }

    public string? Comment { get; init; }
}