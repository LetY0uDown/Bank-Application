using Bank.Core.Objects;
using Bank.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Core.Tools;

public static class DataProvider
{
    public static void AddUser(User user)
    {
        using DataBaseContext db = new();
        db.Users.Add(user);
        db.SaveChanges();
    }

    public static void RemoveUser(User user) 
    {
        using DataBaseContext db = new();
        db.Users.Remove(user);
        db.SaveChanges();
    }

    public static void UpdateUser(User user)
    {
        using DataBaseContext db = new();
        db.Update(user);
        db.SaveChanges();
    }

    public static List<User> GetUsers()
    {
        List<User> users = new();

        using(DataBaseContext db = new())
        {
            List<User> dbUsers = new(db.Users);
            foreach (var user in dbUsers)
                users.Add(user.SetTransactions());
        }

        return users;
    }

    public static List<Transaction> GetTransactions()
    {
        List<Transaction> transactions = new();

        using(DataBaseContext db = new())
        {
            List<Transaction> dbTransactions = new(db.Transactions);
            foreach (var transaction in dbTransactions)
            {
                transaction.Sender = GetUserByID(transaction.SenderID);
                transaction.Reciever = GetUserByID(transaction.RecieverID);

                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    private static User GetUserByID(int id)
    {
        List<User> users;

        using (DataBaseContext db = new())
        {
            users = new(db.Users);
        }

        return (from user in users where user.ID == id select user).FirstOrDefault()!;
    }

    private static User SetTransactions(this User user)
    {
        foreach (var transaction in user.Transactions!)
        {
            if(transaction.SenderID == user.ID)
                user.SendedTransactions!.Add(transaction);

            else
                user.RecievedTransactions!.Add(transaction);
        }

        return user;
    }
}