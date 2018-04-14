using System;
using Microsoft.Data.Sqlite;

namespace VKM.Admin.Providers
{
    public class SqLiteDatabaseProviderBase
    {
        protected readonly string DatabaseConnectionString;

        public SqLiteDatabaseProviderBase(string databaseConnectionString)
        {
            var rootPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            DatabaseConnectionString = databaseConnectionString.Replace("|DataDirectory|", rootPath);
        }
        
        protected int ExecuteNonQueryInternal(string sql)
        {
            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}