using System.Collections.Generic;
using VKM.Admin.Models.Database;

namespace VKM.Admin.Services.Interfaces
{
    public interface IDatabaseProvider
    {
        IEnumerable<Team> LoadAllTeams();
        void SaveTeam(int number);
        void UpdateTeam(Team team);
        Student LoadStudentById(int studentId);
        void SaveStudent(string firstName, string lastName, string middleName, string group, int teamId, double averageValue);
        void UpdateStudent(Student student);
    }
}