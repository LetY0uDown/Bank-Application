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

        db.Users.Update(user); 

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

        return user!;
    }
}