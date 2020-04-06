using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams(
               Expression<Func<Team, bool>> filter = null,
               Func<IQueryable<Team>, IOrderedQueryable<Team>> orderBy = null);
        Team GetTeamByID(int teamId);
        void InsertTeam(Team team);
        void DeleteTeam(int teamId);
        void UpdateTeam(Team team);
    }
}
