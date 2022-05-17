using MySql.Data.MySqlClient;
using System;

namespace Bank.Core.Tools;

public static class SqlDataReaderExtensions
{
    public static string SafeGetString(this MySqlDataReader reader, string columnName)
    {
        int index = reader.GetOrdinal(columnName);

        if (!reader.IsDBNull(index))
            return reader.GetString(index);

        return string.Empty;
    }

    public static Guid SafeGetGuid(this MySqlDataReader reader, string columnName)
    {
        int index = reader.GetOrdinal(columnName);

        if (!reader.IsDBNull(index))
            return reader.GetGuid(index);

        return Guid.Empty;
    }
}