using Bank.Core.Objects.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("Payment")]
public sealed class Payment : Entity
{
    public Payment(string type) => Type = type;
    
    [NotMapped]
    public static List<string> PaymentTypes { get; } = new()
    {
        "Переводы",
        "Животные",
        "Транспорт",
        "Ком. Услуги",
        "Медицина",
        "Образование",
        "Одежда",
        "Отдых",
        "Техника",
        "Еда",
    };

    public decimal Sum { get; set; }
    public string Type { get; private init; }

    public override string ToString() => Type;
}