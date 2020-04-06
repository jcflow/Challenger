using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository.EF
{
    public class EFTeamRepository : EFRepository<Team>, ITeamRepository
    {
        public EFTeamRepository(ChallengerContext challengerContext) : base(challengerContext)
        {
        }

        public void DeleteTeam(int teamId)
        {
            base.Delete(teamId);
        }

        public IEnumerable<Team> GetTeams(Expression<Func<Team, bool>> filter = null,
            Func<IQueryable<Team>, IOrderedQueryable<Team>> orderBy = null)
        {
            return base.Get(filter, orderBy);
        }

        public Team GetTeamByID(int teamId)
        {
            return base.GetByID(teamId);
        }

        public void InsertTeam(Team team)
        {
            base.Insert(team);
        }

        public void UpdateTeam(Team team)
        {
            base.Update(team);
        }
    }
}
