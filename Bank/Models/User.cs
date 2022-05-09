using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models;

[Table("users")]
public class User : Entity
{
    public User() { }
    public User(decimal balance, decimal wasted, decimal recieved)
    {
        Balance = balance;
        WastedMoney = wasted;
        RecievedMoney = recieved;
    }

    [Column(nameof(PhoneNumber))]   public string?  PhoneNumber     { get; set; }
    [Column(nameof(Password))]      public string?  Password        { get; init; }

    [Column(nameof(FirstName))]     public string?  FirstName       { get; set; }
    [Column(nameof(Surname))]       public string?  Surname         { get; set; }
    [Column(nameof(LastName))]      public string?  LastName        { get; set; }

    [Column(nameof(Birthday))]      public string?  Birthday        { get; set; }

    [Column(nameof(Balance))]       public decimal  Balance         { get; private set; }

    [Column(nameof(RecievedMoney))] public decimal  RecievedMoney   { get; private set; }
    [Column(nameof(WastedMoney))]   public decimal  WastedMoney     { get; private set; }

    [Column(nameof(IsBanned))]      public bool     IsBanned        { get; set; }

    public List<Payment>? Payments { get; private set; }

    public List<Transaction> SendedTransactions { get; } = new();
    public List<Transaction> RecievedTransactions { get; } = new();

    public override string ToString() => $"{Surname} {FirstName![0]}. {LastName![0]}.";

    public bool Pay(string type, decimal sum)
    {
        if (Balance - sum < decimal.Zero)
            return false;

        var index = Payment.PaymentTypes.IndexOf(type);

        if (index < 0) return false;

        Payments![index].Sum += sum;
        Balance -= sum;
        WastedMoney += sum;

        DataProvider.Update(this);
        DataProvider.Update(Payments[index]);

        return true;
    }

    public bool SendTransaction(User reciever, decimal sum, string? message = null)
    {
        if (Balance - sum < decimal.Zero)
            return false;

        Transaction transaction = new()
        {
            Sender = this,
            SenderID = ID,
            Reciever = reciever,
            RecieverID = reciever.ID,
            Sum = sum,
            Message = message
        };

        Balance -= sum;
        WastedMoney += sum;

        var index = Payment.PaymentTypes.IndexOf("Переводы");
        Payments![index].Sum += sum;
        DataProvider.Update(Payments[index]);

        SendedTransactions.Add(transaction);
        DataProvider.Insert(transaction);

        reciever.RecieveTransaction(transaction);
        DataProvider.Update(this);

        return true;
    }

    public void SetTransactions(List<Transaction> transactions)
    {
        foreach (var t in transactions)
        {
            if (t.SenderID.Equals(ID))
                SendedTransactions.Add(t);

            else if (t.RecieverID.Equals(ID))
                RecievedTransactions.Add(t);
        }
    }

    public void InitPayments(bool isFirstTime)
    {
        if (isFirstTime)
            InsertPayments();        
        else
            Payments = DataProvider.GetPayments(this);
    }

    public void DepositMoney(decimal sum)
    {
        Balance += sum;
        RecievedMoney += sum;
    }

    private void RecieveTransaction(Transaction transaction)
    {
        Balance += transaction.Sum;
        RecievedMoney += transaction.Sum;
        DataProvider.Update(this);
    }

    private void InsertPayments()
    {
        Payments = new()
        {
            new(ID, "Переводы"),
            new(ID, "Животные"),
            new(ID, "Транспорт"),
            new(ID, "Ком. Услуги"),
            new(ID, "Медицина"),
            new(ID, "Образование"),
            new(ID, "Одежда"),
            new(ID, "Отдых"),
            new(ID, "Техника"),
            new(ID, "Еда")
        };

        foreach (var p in Payments!)
            DataProvider.Insert(p);
    }
}