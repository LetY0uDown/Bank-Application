using Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Objects;

public class DataBaseContext : DbContext
{
    public DataBaseContext() => Database.EnsureCreated();

    private const string _connectionString = "server=localhost;user=root;password=password;database=BankDataBase";

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
    }
}