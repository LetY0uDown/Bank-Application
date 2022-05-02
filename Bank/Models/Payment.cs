using Bank.Core.Objects.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Payment")]
public sealed class Payment : Entity
{
    public Payment(PaymentType type)
    {
        Sum = 0;
        Type = _paymentTypes[type];
    }
    [NotMapped]
    private static readonly Dictionary<PaymentType, string> _paymentTypes = new()
    {
        [PaymentType.Transactions] = "Переводы",
        [PaymentType.Animals] = "Животные",
        [PaymentType.Transport] = "Транспорт",
        [PaymentType.Utilities] = "Коммунальные услуги",
        [PaymentType.Medicine] = "Медицина",
        [PaymentType.Education] = "Образование",
        [PaymentType.Clothes] = "Одежда",
        [PaymentType.Rest] = "Отдых",
        [PaymentType.Technic] = "Техника",
        [PaymentType.Food] = "Еда",
    };

    public decimal Sum { get; set; }
    public string Type { get; private init; }
}

public enum PaymentType
{
    Transactions,
    Animals,
    Transport,
    Utilities,
    Medicine,
    Education,
    Clothes,
    Rest,
    Technic,
    Food
}