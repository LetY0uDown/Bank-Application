using Bank.Core.Objects;
using Bank.Core.Objects.Abstract;
using Bank.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Bank.Core.Tools;

public static class DataProvider
{
    public static List<Payment> GetPayments(Guid ID)
    {
        List<Payment> payments = new();
        var table = typeof(Payment).Name.ToLower() + "s";
        string query = $"SELECT * FROM {table} WHERE UserID = '{ID}' ORDER BY Number";

        if (!Database.OpenConnection()) return null!;

        using (MySqlCommand mc = new(query, Database.SqlConnection))
        using (MySqlDataReader dr = mc.ExecuteReader())
        {
            while (dr.Read())
            {
                Payment payment = new(dr.GetGuid(nameof(payment.ID)), dr.GetString(nameof(payment.Type)));

                payment.Sum = dr.GetDecimal(nameof(payment.Sum));
                payment.UserID = ID;

                payments.Add(payment);
            }
        }

        Database.CloseConnection();

        if (payments.Count == 0) payments = null!;

        return payments;
    }

    public static User TryGetUserByPhoneNumber(string phoneNumber)
    {
        User? user = null;
        var table = typeof(User).Name.ToLower() + "s";
        string query = $"SELECT * FROM {table} WHERE PhoneNumber = '{phoneNumber}'";

        if (!Database.OpenConnection()) return null!;

        using (MySqlCommand mc = new(query, Database.SqlConnection))
        using (MySqlDataReader dr = mc.ExecuteReader())
        {
            if (!dr.HasRows) return null!;

            dr.Read();

            user = new(dr.GetDecimal(nameof(user.Balance)),
                       dr.GetDecimal(nameof(user.WastedMoney)),
                       dr.GetDecimal(nameof(user.RecievedMoney)),
                       dr.GetString(nameof(user.Password)));

            user.ID = dr.GetGuid(nameof(user.ID));
            user.FirstName = dr.GetString(nameof(user.FirstName));
            user.Surname = dr.GetString(nameof(user.Surname));
            user.LastName = dr.GetString(nameof(user.LastName));
            user.Birthday = dr.GetString(nameof(user.Birthday));
            user.PhoneNumber = dr.GetString(nameof(user.PhoneNumber));
            user.IsBanned = dr.GetBoolean(nameof(user.IsBanned));            
        }

        Database.CloseConnection();

        user.InitPayments(false);

        return user;
    }

    public static List<Transaction> GetTransactions(Guid id)
    {
        List<Transaction> transactions = new();
        var table = typeof(Transaction).Name.ToLower() + "s";
        string query = $"SELECT * FROM {table} WHERE RecieverID = '{id}' OR SenderID = '{id}'";

        if (!Database.OpenConnection()) return null!;

        using (MySqlCommand mc = new(query, Database.SqlConnection))
        using (MySqlDataReader dr = mc.ExecuteReader())
        {
            while (dr.Read())
            {
                Transaction transaction = new();

                transaction.ID = dr.GetGuid(nameof(transaction.ID));
                transaction.SenderID = dr.SafeGetGuid(nameof(transaction.SenderID));
                transaction.RecieverID = dr.SafeGetGuid(nameof(transaction.RecieverID));
                transaction.Sum = dr.GetDecimal(nameof(transaction.Sum));
                transaction.Message = dr.GetString(nameof(transaction.Message));

                transactions.Add(transaction);
            }
        }

        Database.CloseConnection();

        foreach (var t in transactions)
        {
            t.Sender = TryGetUserByID(t.SenderID);
            t.Reciever = TryGetUserByID(t.RecieverID);

            if (t.Sender is null)
                t.Sender = User.Empty;
        }

        return transactions;
    }

    private static User TryGetUserByID(Guid id)
    {
        User? user = null;
        var table = typeof(User).Name.ToLower() + "s";
        string query = $"SELECT * FROM {table} WHERE ID = '{id}'";

        if (!Database.OpenConnection()) return null!;

        using (MySqlCommand mc = new(query, Database.SqlConnection))
        using (MySqlDataReader dr = mc.ExecuteReader())
        {
            if (!dr.HasRows) return null!;

            dr.Read();

            user = new(dr.GetDecimal(nameof(user.Balance)),
                       dr.GetDecimal(nameof(user.WastedMoney)),
                       dr.GetDecimal(nameof(user.RecievedMoney)),
                       dr.GetString(nameof(user.Password)));

            user.ID = dr.GetGuid(nameof(user.ID));
            user.FirstName = dr.GetString(nameof(user.FirstName));
            user.Surname = dr.GetString(nameof(user.Surname));
            user.LastName = dr.GetString(nameof(user.LastName));
            user.Birthday = dr.GetString(nameof(user.Birthday));
            user.PhoneNumber = dr.GetString(nameof(user.PhoneNumber));
            user.IsBanned = dr.GetBoolean(nameof(user.IsBanned));
        }

        Database.CloseConnection();

        return user;
    }

    public static void Insert<T>(T value) where T : Entity
    {
        GetMetaData(value, out string table, out List<(string, object)> values);

        var query = CreateInsertQuery(table, values);

        Database.ExecuteNonQuery(query.Item1, query.Item2);
    }
    public static void Update<T>(T value) where T : Entity
    {
        GetMetaData(value, out string table, out List<(string, object)> values);

        var query = CreateUpdateQuery(table, values, value.ID);

        Database.ExecuteNonQuery(query.Item1, query.Item2);
    }
    public static void Delete<T>(T value) where T : Entity
    {
        var type = value.GetType();

        string table = GetTableName(type);

        string query = $"delete from `{table}` where id = '{value.ID}'";

        Database.ExecuteNonQuery(query);
    }
    public static int GetNumRows(Type type)
    {
        string table = GetTableName(type);
        return Database.GetRowsCount(table);
    }
    private static string GetTableName(Type type)
    {
        var tableAtrributes = type.GetCustomAttributes(typeof(TableAttribute), false);

        return (tableAtrributes.First() as TableAttribute)!.Name;
    }
    private static (string, MySqlParameter[]) CreateInsertQuery(string table, List<(string, object)> values)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"INSERT INTO `{table}` set ");

        List<MySqlParameter> parameters = InitParameters(values, stringBuilder);

        return (stringBuilder.ToString(), parameters.ToArray());
    }
    private static (string, MySqlParameter[]) CreateUpdateQuery(string table, List<(string, object)> values, Guid id)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"UPDATE `{table}` SET ");

        List<MySqlParameter> parameters = InitParameters(values, stringBuilder);

        stringBuilder.Append($" WHERE id = '{id}'");

        return (stringBuilder.ToString(), parameters.ToArray());
    }
    private static List<MySqlParameter> InitParameters(List<(string, object)> values, StringBuilder stringBuilder)
    {
        List<MySqlParameter> parameters = new();

        int count = 1;

        var rows = values.Select(s =>
        {
            parameters.Add(new($"p{count}", s.Item2));
            return $"{s.Item1} = @p{count++}";
        });

        stringBuilder.Append(string.Join(',', rows));

        return parameters;
    }
    private static void GetMetaData<T>(T value, out string table, out List<(string, object)> values)
    {
        var type = value!.GetType();

        var tableAtrributes = type.GetCustomAttributes(typeof(TableAttribute), false);

        table = (tableAtrributes.First() as TableAttribute)!.Name;

        values = new();

        var props = type.GetProperties();

        foreach (var prop in props)
        {
            var columnAttributes = prop.GetCustomAttributes(typeof(ColumnAttribute), false);

            if (columnAttributes.Length > 0)
            {
                string column = (columnAttributes.First() as ColumnAttribute)!.Name!;
                values.Add(new(column, prop.GetValue(value)!));
            }
        }
    }
}
