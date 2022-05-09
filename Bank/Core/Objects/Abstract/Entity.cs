using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Objects.Abstract;

public abstract class Entity
{
    [Column(nameof(ID))]
    public System.Guid ID { get; set; } = System.Guid.NewGuid();
}