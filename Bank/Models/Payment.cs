using Bank.Core.Objects.Abstract;
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("payments")]
public class Payment : Entity
{
    public Payment(string type)
    {
        Type = type;
        Number = PaymentTypes.IndexOf(Type!);
    }
    public Payment(Guid id, string type) : this(type) => UserID = id;

    [Column(nameof(UserID))]
    public Guid UserID { get; set; }

    [Column(nameof(Type))]
    public string? Type { get; set; }

    [Column(nameof(Sum))]
    public decimal Sum { get; set; }

    [Column(nameof(Number))]
    public int Number { get; private init; }

    public static ImmutableList<string> PaymentTypes { get; } = ImmutableList.CreateRange(new string[]
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
        "Еда"
    });

    public override string ToString() => Type!;
}