using Bank.Core.Objects.Abstract;
using Bank.Core.Tools;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Bank.Models;

[Table("users")]
public class User : Entity
{
    public User(string pass) => Password = pass;
    public User(decimal balance, decimal wasted, decimal recieved, string pass)
    {
        Balance = balance;
        WastedMoney = wasted;
        RecievedMoney = recieved;
        Password = pass;
    }

    public static User Empty { get; } = new(0M, 0M, 0M, string.Empty)
    {
        FirstName = null,
        Surname = "Удалено",
        LastName = null
    };

    [Column(nameof(PhoneNumber))]   public string?  PhoneNumber     { get; set; }
    [Column(nameof(Password))]      public string?  Password        { get; private set; }

    [Column(nameof(FirstName))]     public string?  FirstName       { get; set; }
    [Column(nameof(Surname))]       public string?  Surname         { get; set; }
    [Column(nameof(LastName))]      public string?  LastName        { get; set; }

    [Column(nameof(Birthday))]      public string?  Birthday        { get; set; }

    [Column(nameof(Balance))]       public decimal  Balance         { get; private set; }

    [Column(nameof(RecievedMoney))] public decimal  RecievedMoney   { get; private set; }
    [Column(nameof(WastedMoney))]   public decimal  WastedMoney     { get; private set; }

    [Column(nameof(IsBanned))]      public bool     IsBanned        { get; set; }

    public static Regex BirthdayRegex { get; } = new(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
    public static Regex PhoneNumberRegex { get; } = new(@"^\+?[0-9]-?[0-9]{3}-?[0-9]{3}-?[0-9]{2}-?[0-9]{2}$");
    public static Regex NameRegex { get; } = new(@"^[А-Я][а-я]+$");
    public static Regex PasswordRegex { get; } = new(@"^[A-Za-z1234567890!@#$%^&*\-_+=]{6,20}$");

    public List<Payment>? Payments { get; private set; }

    public List<Transaction>? SendedTransactions { get; private set; }
    public List<Transaction>? RecievedTransactions { get; private set; } 

    public override string ToString() =>  $"{Surname} {FirstName?[0]}. {LastName?[0]}.";

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
            Message = message ?? string.Empty
        };

        Balance -= sum;
        WastedMoney += sum;

        var index = Payment.PaymentTypes.IndexOf("Переводы");
        Payments![index].Sum += sum;
        DataProvider.Update(Payments[index]);

        SendedTransactions!.Add(transaction);
        DataProvider.Insert(transaction);

        reciever.RecieveTransaction(transaction);
        DataProvider.Update(this);

        return true;
    }

    public void SetTransactions(List<Transaction> transactions)
    {
        SendedTransactions = new();
        RecievedTransactions = new();

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
            Payments = DataProvider.GetPayments(ID);
    }

    public void DepositMoney(decimal sum)
    {
        Balance += sum;
        RecievedMoney += sum;
    }

    public bool ChangePassword(string oldPass, string newPass)
    {
        if (oldPass.Equals(Password))
        {
            Password = newPass;

            Properties.Settings.Default.SavedPassword = Password;
            Properties.Settings.Default.Save();

            DataProvider.Update(this);

            return true;
        }

        return false;
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
