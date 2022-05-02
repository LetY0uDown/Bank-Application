using Bank.Core.Objects;
using Bank.Models;
using System;
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

    public static void UpdateUser(User user)
    {
        using DataBaseContext db = new();

        db.Update(user);

        db.SaveChanges();
    }

    public static User TryGetUserByPhoneNumber(string phoneNumber)
    {
        List<User> users;

        using (DataBaseContext db = new())
        {
            users = new(db.Users);
        }

        var user = (from u in users where u.PhoneNumber.Equals(phoneNumber) select u).FirstOrDefault()!;

        user?.SetTransactions();

        return user;
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
        foreach (var transaction in user.Transactions)
        {
            if (transaction.SenderID.Equals(user.ID))
                user.SendedTransactions!.Add(transaction);

            else if (transaction.RecieverID.Equals(user.ID))
                user.RecievedTransactions!.Add(transaction);
        }

        return user;
    }
}