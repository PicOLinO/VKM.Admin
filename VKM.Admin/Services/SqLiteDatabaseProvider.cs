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
        
        public IEnumerable<Team> LoadAllTeams()
        {
            var sql = @"SELECT 
                            [Teams].[ID] as [TeamID],
                            [Teams].[Number] as [TeamNumber],
                            [Students].[ID] as [StudentID],
                            [Students].[FirstName],
                            [Students].[LastName],
                            [Students].[MiddleName],
                            [Students].[UniversityGroup],
                            [Students].[TeamID],
                            [Students].[AverageScore] 
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
                    var reader = cmd.ExecuteReader();
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
                            MiddleName = reader["MiddleName"].ToString(),
                            Group = reader["UniversityGroup"].ToString(),
                            AverageValue = double.Parse(reader["AverageScore"].ToString()),
                            Team = currentTeam
                        };
                        
                        currentTeam.Students.Add(student);
                        
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
                using (var cmd = new SqliteCommand(sql))
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
                using (var cmd = new SqliteCommand(sql))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Student LoadStudentById(int studentId)
        {
            var sql = $"SELECT * FROM [Students] WHERE [ID] = {studentId}";
            
            using (var connection = new SqliteConnection(databaseConnectionString))
            {
                connection.Open();
                using (var cmd = new SqliteCommand(sql))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Student
                        {
                            Id = int.Parse(reader["ID"].ToString()),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            MiddleName = reader["MiddleName"].ToString(),
                            Group = reader["UniversityGroup"].ToString(),
                            AverageValue = double.Parse(reader["AverageScore"].ToString())
                        };
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
                using (var cmd = new SqliteCommand(sql))
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
                using (var cmd = new SqliteCommand(sql))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}