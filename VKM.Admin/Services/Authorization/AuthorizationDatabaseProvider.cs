using System;
using Microsoft.Data.Sqlite;
using VKM.Admin.Models.Database;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Services.Authorization
{
    public class AuthorizationDatabaseProvider : SqLiteDatabaseProviderBase, IAuthorizationDatabaseProvider
    {
        public AuthorizationDatabaseProvider(string databaseConnectionString) : base(databaseConnectionString)
        {
        }
        
        public bool Authorize(string userName, string password)
        {
            var sql = $"SELECT [Login], [PasswordHash] FROM [User] WHERE [Login] = '{userName}'";

            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                Login = reader["Login"].ToString(),
                                Password = reader["PasswordHash"].ToString()
                            };

                            var isPasswordsEquals = PasswordHasher.IsEqual(user.Password, password);
                            if (!string.IsNullOrEmpty(user.Login) && isPasswordsEquals)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void Register(string userName, string password, int studentId)
        {
            var hashedPassword = PasswordHasher.Hash(password);
            var sql = $"INSERT INTO [User] (Login, PasswordHash, StudentID) VALUES ('{userName}', '{hashedPassword}', {studentId})";
            ExecuteNonQueryInternal(sql);
        }

        public void ResetPassword(string userName, string newPassword)
        {
            var sql = $"UPDATE [User] SET [PasswordHash] = '{PasswordHasher.Hash(newPassword)}' WHERE [Login] = '{userName}'";
            ExecuteNonQueryInternal(sql);
        }
    }
}