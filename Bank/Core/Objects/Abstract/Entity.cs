using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Core.Objects.Abstract;

public abstract class Entity
{
    public Guid ID { get; init; } 
}