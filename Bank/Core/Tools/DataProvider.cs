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

        //using (DataBaseContext db = new())
        //{
        //    List<Transaction> transactions = new(from t in db.Transactions where t.SenderID.Equals(user.ID) || t.RecieverID.Equals(user.ID) select t);
        //    foreach (var t in transactions)
        //        user.Transactions.Add(t);
        //}

        return user!;
    }
}