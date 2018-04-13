using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using VKM.Admin.Models;
using VKM.Admin.Models.Database;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Services
{
    public class SqLiteDatabaseProvider : SqLiteDatabaseProviderBase, IDatabaseProvider
    {
        public SqLiteDatabaseProvider(string databaseConnectionString) : base(databaseConnectionString)
        {
        }

        public IEnumerable<Team> LoadTeamsAndStudents()
        {
            var sql = @"SELECT 
                            [Team].[ID] as [TeamID],
                            [Team].[Number] as [TeamNumber],
                            [Student].[ID] as [StudentID],
                            [Student].[FirstName],
                            [Student].[LastName],
                            [Student].[MiddleName]
                        FROM 
                            [Team] 
                                LEFT JOIN [Student] 
                                    ON [Team].[ID] = [Student].[TeamID]";

            var teams = new List<Team>();
            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var teamId = int.Parse(reader["TeamID"].ToString());
                            var currentTeam = teams.SingleOrDefault(t => t.Id == teamId);
                            if (currentTeam is null)
                            {
                                currentTeam = new Team
                                {
                                    Id = teamId,
                                    Number = int.Parse(reader["TeamNumber"].ToString()),
                                    Students = new List<Student>()
                                };
                                teams.Add(currentTeam);
                            }

                            var studentId = reader["StudentID"].ToString();
                            if (!string.IsNullOrEmpty(studentId))
                            {
                                var student = new Student
                                {
                                    Id = int.Parse(studentId),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    MiddleName = reader["MiddleName"].ToString()
                                };

                                currentTeam.Students.Add(student);
                            }
                        }
                    }
                }
            }

            return teams;
        }

        public void SaveTeam(int number)
        {
            var sql = $"INSERT INTO [Team] VALUES ({number})";
            ExecuteNonQueryInternal(sql);
        }

        public void UpdateTeam(Team team)
        {
            var sql = $"UPDATE [Team] SET [Number] = {team.Number} WHERE [ID] = {team.Id}";
            ExecuteNonQueryInternal(sql);
        }

        public Student LoadStudentById(int studentId)
        {
            var sql = $@"SELECT [Student].[ID] as [StudentID],
                                [Student].[FirstName],
                                [Student].[LastName],
                                [Student].[MiddleName],
                                [Student].[UniversityGroup],
                                [Student].[AverageScore],
                                [Team].[ID] as [TeamID],
                                [Team].[Number] as [TeamNumber]
                            FROM [Student] 
                            INNER JOIN [Team] 
                                ON [Student].[TeamID] = [Team].[ID] 
                            WHERE [Student].[ID] = {studentId}";

            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new Student
                            {
                                Id = int.Parse(reader["StudentID"].ToString()),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                Group = reader["UniversityGroup"].ToString(),
                                AverageValue = double.Parse(reader["AverageScore"].ToString()),
                                Team = new Team
                                {
                                    Id = int.Parse(reader["TeamID"].ToString()),
                                    Number = int.Parse(reader["TeamNumber"].ToString())
                                }
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void SaveStudent(string firstName, string lastName, string middleName, string group, int teamId,
            double averageValue)
        {
            var sql = $"INSERT INTO [Student] VALUES ({firstName}, {lastName}, {middleName}, {group}, {teamId}, {averageValue})";

            ExecuteNonQueryInternal(sql);
        }

        public void UpdateStudent(Student student)
        {
            var sql = $@"UPDATE [Student] SET  [FirstName] = '{student.FirstName}', 
                                                [LastName] = '{student.LastName}', 
                                                [MiddleName] = '{student.MiddleName}', 
                                                [UniversityGroup] = '{student.Group}', 
                                                [TeamID] = {student.Team.Id}
                         WHERE [ID] = {student.Id}";

            ExecuteNonQueryInternal(sql);
        }

        public IEnumerable<HistoryItem> LoadHistoryByStudentId(int studentId)
        {
            var sql = $@"SELECT [History].[Algorithm],
                                [History].[Date],
                                [History].[Value]
                            FROM [History]
                            WHERE [History].[StudentID] = {studentId}";

            var history = new List<HistoryItem>();
            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var historyItem = new HistoryItem
                            {
                                AlgorithmName = reader["Algorithm"].ToString(),
                                Date = DateTime.Parse(reader["Date"].ToString()),
                                Value = int.Parse(reader["Value"].ToString())
                            };
                            history.Add(historyItem);
                        }
                    }
                }
            }

            return history;
        }

        public void RemoveStudentById(int id)
        {
            var sql = $@"DELETE FROM [Student] WHERE [ID] = {id}";
            ExecuteNonQueryInternal(sql);
        }

        public IEnumerable<Team> LoadTeams()
        {
            var sql = @"SELECT 
                            [Team].[ID] as [TeamID],
                            [Team].[Number] as [TeamNumber]
                        FROM 
                            [Team]";

            var teams = new List<Team>();
            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var team = new Team
                            {
                                Id = int.Parse(reader["TeamID"].ToString()),
                                Number = int.Parse(reader["TeamNumber"].ToString()),
                                Students = new List<Student>()
                            };
                            teams.Add(team);
                        }
                    }
                }
            }

            return teams;
        }

        public Team LoadTeam(int id)
        {
            var sql = $"SELECT [Team].[Number] as [TeamNumber] FROM [Team] WHERE [Team].[ID] = {id}";
            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new Team
                            {
                                Id = id,
                                Number = int.Parse(reader["TeamNumber"].ToString())
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void RemoveTeamById(int id)
        {
            var sql = $"DELETE FROM [Student] WHERE [TeamID] = {id}; DELETE FROM [Team] WHERE [ID] = {id}";
            ExecuteNonQueryInternal(sql);
        }

        public int CreateTeam(Team team)
        {
            var sql = $"INSERT INTO [Team] (Number) VALUES ({team.Number}); SELECT [ID] FROM [Team] WHERE [ID] = (SELECT MAX(ID) FROM [Team])";
            
            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return int.Parse(reader["ID"].ToString());
                        }
                    }
                }
            }
            return -1;
        }

        public int CreateStudent(Student student)
        {
            var sql = $@"INSERT INTO [Student] (FirstName, LastName, MiddleName, UniversityGroup, TeamID) 
                                VALUES ('{student.FirstName}', '{student.LastName}', '{student.MiddleName}', '{student.Group}', {student.Team.Id}); 
                         SELECT [ID] FROM [Student] WHERE [ID] = (SELECT MAX(ID) FROM [Student])";
            
            using (var connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return int.Parse(reader["ID"].ToString());
                        }
                    }
                }
            }
            return -1;
        }

        public void AddHistoryItem(HistoryItem historyItem, int studentId)
        {
            var sql = $"INSERT INTO [History] (StudentID, Value, Date, Algorithm) VALUES ({studentId}, {historyItem.Value}, '{historyItem.Date}', '{historyItem.AlgorithmName}')";
            ExecuteNonQueryInternal(sql);
        }
    }
}