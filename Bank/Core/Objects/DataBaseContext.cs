using Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Objects;

public class DataBaseContext : DbContext
{
    public DataBaseContext() => Database.EnsureCreated();

    private const string CONNECTION_STRING = "server=localhost;user=root;password=password;database=BankDataBase";

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(CONNECTION_STRING, ServerVersion.AutoDetect(CONNECTION_STRING));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasMany<Payment>(u => u.Payments);

        modelBuilder.Entity<User>().HasMany<Transaction>(u => u.Transactions).WithOne(t => t.Sender).HasForeignKey(t => t.SenderID);
        modelBuilder.Entity<Transaction>().HasOne(t => t.Reciever);
    }
}