using Microsoft.Data.Sqlite;
using VKM.Admin.Services.Authorization;

namespace VKM.Admin.Providers
{
    public class AuthorizationDatabaseProvider : SqLiteDatabaseProviderBase, IAuthorizationDatabaseProvider
    {
        public AuthorizationDatabaseProvider(string databaseConnectionString) : base(databaseConnectionString)
        {
        }
        
        public int Authorize(string userName, string password)
        {
            var sql = $"SELECT [StudentID], [Login], [PasswordHash] FROM [User] WHERE [Login] = '{userName}'";

            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userId = int.Parse(reader["StudentID"].ToString());
                            var userLogin = reader["Login"].ToString();
                            var userPassword = reader["PasswordHash"].ToString();

                            var isPasswordsEquals = HashPasswordService.IsEqual(userPassword, password);
                            if (!string.IsNullOrEmpty(userLogin) && isPasswordsEquals)
                            {
                                return userId;
                            }
                        }
                    }
                }
            }
            return -1;
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