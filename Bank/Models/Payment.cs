using Bank.Core.Objects.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Payment")]
public sealed class Payment : Entity
{
    public Payment(decimal sum, PaymentType type)
    {
        Sum = sum;
        Type = _paymentTypes[type];
    }
    [NotMapped]
    private static readonly Dictionary<PaymentType, string> _paymentTypes = new()
    {
        [PaymentType.Transactions] = "Transactions"
    };

    public decimal Sum { get; private init; }
    public string Type { get; private init; }
}

public enum PaymentType
{
    Transactions
}