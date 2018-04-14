using Microsoft.Data.Sqlite;
using VKM.Admin.Services.Authorization;

namespace VKM.Admin.Providers
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
                            var userLogin = reader["Login"].ToString();
                            var userPassword = reader["PasswordHash"].ToString();

                            var isPasswordsEquals = HashPasswordService.IsEqual(userPassword, password);
                            if (!string.IsNullOrEmpty(userLogin) && isPasswordsEquals)
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
            var hashedPassword = HashPasswordService.Hash(password);
            var sql = $"INSERT INTO [User] (Login, PasswordHash, StudentID) VALUES ('{userName}', '{hashedPassword}', {studentId})";
            ExecuteNonQueryInternal(sql);
        }

        public void ResetPassword(string userName, string newPassword)
        {
            var sql = $"UPDATE [User] SET [PasswordHash] = '{HashPasswordService.Hash(newPassword)}' WHERE [Login] = '{userName}'";
            ExecuteNonQueryInternal(sql);
        }
    }
}