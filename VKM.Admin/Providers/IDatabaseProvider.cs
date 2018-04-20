using System;
using System.Collections.Generic;
using VKM.Admin.Models.Database;
using VKM.Admin.Models.Database.Domain;

namespace VKM.Admin.Providers
{
    public interface IDatabaseProvider
    {
        IEnumerable<TeamWithParticipants> LoadTeamsAndStudents();
        void UpdateTeam(int teamId, int teamNumber);
        Student LoadStudentById(int studentId);
        void UpdateStudent(int studentId, string firstName, string lastName, string middleName, string group, int teamId);
        IEnumerable<HistoryItem> LoadHistoryByStudentId(int studentId);
        void RemoveStudentById(int id);
        IEnumerable<Team> LoadTeams();
        Team LoadTeam(int id);
        void RemoveTeamById(int id);
        int CreateTeam(int teamNumber);
        int CreateStudent(string firstName, string lastName, string middleName, string group, int teamId);
        void AddHistoryItem(string algorithmName, DateTime date, int value, int studentId);
        IEnumerable<TeamWithParticipants> LoadTeamsWithStudentsWithoutLogins();
    }
}