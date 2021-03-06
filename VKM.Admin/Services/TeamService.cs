﻿using System.Collections.Generic;
using VKM.Admin.Models.Database;
using VKM.Admin.Models.Database.Domain;
using VKM.Admin.Models.ViewModel.Team;
using VKM.Admin.Providers;

namespace VKM.Admin.Services
{
    public class TeamService : DatabaseService
    {
        public TeamService(IDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public IEnumerable<TeamWithParticipants> GetAllTeamsWithStudents()
        {
            var teamsWithStudents = DatabaseProvider.LoadTeamsAndStudents();
            return teamsWithStudents;
        }

        public IEnumerable<TeamWithParticipants> GetAllTeamsWithStudentsWithoutLogins()
        {
            var teamsWithStudentsWithoutLogins = DatabaseProvider.LoadTeamsWithStudentsWithoutLogins();
            return teamsWithStudentsWithoutLogins;
        }

        public IEnumerable<Team> GetAllTeams()
        {
            var teams = DatabaseProvider.LoadTeams();
            return teams;
        }
        
        public Team GetTeamById(int id)
        {
            var team = DatabaseProvider.LoadTeam(id);
            return team;
        }

        public int CreateTeam(TeamCreateViewModel vm)
        {
            var id = DatabaseProvider.CreateTeam(vm.Number);
            return id;
        }

        public void UpdateTeam(TeamUpdateViewModel vm)
        {
            DatabaseProvider.UpdateTeam(vm.Id, vm.Number);
        }

        public void DeleteTeamById(int id)
        {
            DatabaseProvider.RemoveTeamById(id);
        }
    }
}