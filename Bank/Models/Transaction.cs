using Bank.Core.Objects.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Transaction")]
public sealed class Transaction : Entity
{
    public int SenderID { get; set; }
    public User? Sender { get; set; }

    public int RecieverID { get; set; }
    public User? Reciever { get; set; }

    [DataType("DECIMAL(65, 10)")]
    public decimal Sum { get; init; }

    public string? Message { get; init; }
}