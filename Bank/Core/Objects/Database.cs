using Bank.Properties;
using MySql.Data.MySqlClient;
using System;
using Bank.Core.Tools;

namespace Bank.Core.Objects;

public static class Database
{
    static Database()
    {
        MySqlConnectionStringBuilder builder = new();
        builder.UserID = Settings.Default.DBUser;
        builder.Password = Settings.Default.DBPassword;
        builder.Database = Settings.Default.DataBase;
        builder.Server = Settings.Default.DBServer;
        builder.CharacterSet = "utf8";
        builder.ConnectionTimeout = 5;

        SqlConnection = new(builder.GetConnectionString(true));
    }

    public static MySqlConnection? SqlConnection { get; private set; } = null;

    public static bool OpenConnection()
    {
        try
        {
            if (SqlConnection!.Ping())
                return true;

            SqlConnection!.Open();

            return true;
        }
        catch (Exception e)
        {
            WarningBox.Show("Ошибка базы данных", e.Message);
        }

        return false;
    }

    public static void CloseConnection()
    {
        try
        {
            SqlConnection!.Close();
        }
        catch (Exception e)
        {
            WarningBox.Show("Ошибка базы данных", e.Message);
        }
    }

    public static void ExecuteNonQuery(string query, MySqlParameter[]? parameters = null)
    {
        if (OpenConnection())
        {
            using (MySqlCommand mc = new(query, SqlConnection))
            {
                if (parameters is not null)
                    mc.Parameters.AddRange(parameters);

                mc.ExecuteNonQuery();
            }
            CloseConnection();
        }
    }
    public static int GetRowsCount(string table)
    {
        string column = "Rows";
        return GetTableInfo(table, column);
    }

    private static int GetTableInfo(string table, string column)
    {
        int result = 0;

        if (OpenConnection())
        {
            using (MySqlCommand mc = new($"SHOW TABLE STATUS WHERE `Name` = '{table}'", SqlConnection))
            using (MySqlDataReader dr = mc.ExecuteReader())
            {
                dr.Read();
                result = dr.GetInt32(column);
            }
            CloseConnection();
        }
        return result;
    }
}