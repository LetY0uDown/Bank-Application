using Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Objects;

public class DataBaseContext : DbContext
{
    public DataBaseContext() => Database.EnsureCreated();

    private const string CONNECTION_STRING = "server=localhost;user=root;password=password;database=BankDataBase";

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(CONNECTION_STRING, ServerVersion.AutoDetect(CONNECTION_STRING));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>().HasOne(t => t.Sender);
        modelBuilder.Entity<Transaction>().HasOne(t => t.Reciever);
    }
}