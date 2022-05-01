using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Core.Tools;

public static class DataProvider
{
    public static void AddEntity<T>(Entity entity) where T : Entity
    {
        using DataBaseContext db = new();

        if (typeof(T) == typeof(User))
            db.Users.Add((User)entity);

        else if (typeof(T) == typeof(Transaction))
            db.Transactions.Add((Transaction)entity);

        db.SaveChanges();
    }

    public static void RemoveEntity<T>(Entity entity) where T : Entity 
    {
        using DataBaseContext db = new();

        if (typeof(T) == typeof(User))
            db.Users.Remove((User)entity);

        else if (typeof (T) == typeof(Transaction))
            db.Transactions.Remove((Transaction)entity);

        db.SaveChanges();
    }

    public static void UpdateEntity<T>(Entity entity) where T : Entity
    {
        using DataBaseContext db = new();
        db.Update(entity);
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

    public static User TryGetUserByPhoneNumber(string phoneNumber)
    {
        List<User> users;

        using (DataBaseContext db = new())
        {
            users = new(db.Users);
        }

        return (from user in users where user.PhoneNumber.Equals(phoneNumber) select user).FirstOrDefault()!;
    }

    private static User GetUserByID(Guid id)
    {
        List<User> users;

        using (DataBaseContext db = new())
        {
            users = new(db.Users);
        }

        return (from user in users where user.ID.Equals(id) select user).FirstOrDefault()!;
    }

    private static User SetTransactions(this User user)
    {
        foreach (var transaction in GetTransactions())
        {
            if (transaction.SenderID.Equals(user.ID))
                user.SendedTransactions!.Add(transaction);

            else if (transaction.RecieverID.Equals(user.ID))
                user.RecievedTransactions!.Add(transaction);
        }

        return user;
    }
}