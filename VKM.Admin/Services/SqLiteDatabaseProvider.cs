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
    public class SqLiteDatabaseProvider : IDatabaseProvider
    {
        private readonly string databaseConnectionString;
        
        public SqLiteDatabaseProvider(string databaseConnectionString)
        {
            var rootPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            this.databaseConnectionString = databaseConnectionString.Replace("|DataDirectory|", rootPath);
        }
        
        public IEnumerable<Team> LoadTeamsAndStudents()
        {
            var sql = @"SELECT 
                            [Teams].[ID] as [TeamID],
                            [Teams].[Number] as [TeamNumber],
                            [Students].[ID] as [StudentID],
                            [Students].[FirstName],
                            [Students].[LastName],
                            [Students].[MiddleName]
                        FROM 
                            [Teams] 
                                INNER JOIN [Students] 
                                    ON [Teams].[ID] = [Students].[TeamID]";

            var teams = new List<Team>();
            using (var connection = new SqliteConnection(databaseConnectionString))
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

                            var student = new Student
                                          {
                                              Id = int.Parse(reader["StudentID"].ToString()),
                                              FirstName = reader["FirstName"].ToString(),
                                              LastName = reader["LastName"].ToString(),
                                              MiddleName = reader["MiddleName"].ToString()
                                          };

                            currentTeam.Students.Add(student);

                        }
                    }
                }
            }

            return teams;
        }

        public void SaveTeam(int number)
        {
            var sql = $"INSERT INTO [Teams] VALUES ({number})";
            
            using (var connection = new SqliteConnection(databaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTeam(Team team)
        {
            var sql = $"UPDATE [Teams] SET [Number] = {team.Number} WHERE [ID] = {team.Id}";
            
            using (var connection = new SqliteConnection(databaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Student LoadStudentById(int studentId)
        {
            var sql = $@"SELECT [Students].[ID] as [StudentID],
                                [Students].[FirstName],
                                [Students].[LastName],
                                [Students].[MiddleName],
                                [Students].[UniversityGroup],
                                [Students].[AverageScore],
                                [Teams].[ID] as [TeamID],
                                [Teams].[Number] as [TeamNumber]
                            FROM [Students] 
                            INNER JOIN [Teams] 
                                ON [Students].[TeamID] = [Teams].[ID] 
                            WHERE [Students].[ID] = {studentId}";
            
            using (var connection = new SqliteConnection(databaseConnectionString))
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
                                                  Id = int.Parse(reader["TeamNumber"].ToString()),
                                                  Number = int.Parse(reader["TeamNumber"].ToString())
                                              }
                                   };
                        }
                    }
                }
            }

            return null;
        }

        public void SaveStudent(string firstName, string lastName, string middleName, string group, int teamId, double averageValue)
        {
            var sql = $"INSERT INTO [Students] VALUES ({firstName}, {lastName}, {middleName}, {group}, {teamId}, {averageValue})";
            
            using (var connection = new SqliteConnection(databaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStudent(Student student)
        {
            var sql = $@"UPDATE [Students] SET  [FirstName] = {student.FirstName}, 
                                                [LastName] = {student.LastName}, 
                                                [MiddleName] = {student.MiddleName}, 
                                                [UniversityGroup] = {student.Group}, 
                                                [TeamID] = {student.Team.Id}, 
                                                [AverageScore] = {student.AverageValue}
                         WHERE [ID] = {student.Id}";
            
            using (var connection = new SqliteConnection(databaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<HistoryItem> LoadHistoryByStudentId(int studentId)
        {
            var sql = $@"SELECT [Histories].[Algorithm],
                                [Histories].[Date],
                                [Histories].[Value]
                            FROM [Histories]
                            WHERE [Histories].[StudentID] = {studentId}";

            var history = new List<HistoryItem>();
            using (var connection = new SqliteConnection(databaseConnectionString))
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
            var sql = $@"DELETE FROM [Students] WHERE [ID] = {id}";

            using (var connection = new SqliteConnection(databaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Team> LoadTeams()
        {
            var sql = @"SELECT 
                            [Teams].[ID] as [TeamID],
                            [Teams].[Number] as [TeamNumber]
                        FROM 
                            [Teams]";

            var teams = new List<Team>();
            using (var connection = new SqliteConnection(databaseConnectionString))
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
    }
}