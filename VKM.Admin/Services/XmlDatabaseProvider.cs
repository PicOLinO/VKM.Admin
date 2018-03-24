using System.Collections.Generic;
using VKM.Admin.Models.Database;
using VKM.Admin.Services.Interfaces;

namespace VKM.Admin.Services
{
    public class XmlDatabaseProvider : IDatabaseProvider
    {
        private readonly string databasePath;
        
        public XmlDatabaseProvider(string databasePath)
        {
            this.databasePath = databasePath;
        }
        
        public IEnumerable<Team> LoadAllTeams()
        {
            throw new System.NotImplementedException();
        }

        public void SaveTeam(int number)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateTeam(Team team)
        {
            throw new System.NotImplementedException();
        }

        public Student LoadStudentById(int studentId)
        {
            throw new System.NotImplementedException();
        }

        public void SaveStudent(string firstName, string lastName, string middleName, Team team, string @group)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateStudent(Student student)
        {
            throw new System.NotImplementedException();
        }
    }
}