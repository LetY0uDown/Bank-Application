using System;

namespace Bank.Core.Objects.Abstract;

public abstract class Entity
{
    public Guid ID { get; init; } = Guid.NewGuid();
}