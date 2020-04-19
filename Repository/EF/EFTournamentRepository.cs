using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository.EF
{
    public class EFTournamentRepository : EFRepository<Tournament>, ITournamentRepository
    {
        public EFTournamentRepository(ChallengerContext challengerContext) : base(challengerContext)
        {
        }

        public void DeleteTournament(int tournamentId)
        {
            base.Delete(tournamentId);
        }

        public Tournament GetTournamentByID(int tournamentId)
        {
            return base.GetByID(tournamentId);
        }

        public IEnumerable<Tournament> GetTournaments(Expression<Func<Tournament, bool>> filter = null,
            Func<IQueryable<Tournament>, IOrderedQueryable<Tournament>> orderBy = null)
        {
            return base.Get(filter, orderBy);
        }

        public Tournament InsertTournament(Tournament tournament)
        {
            return base.Insert(tournament);
        }

        public void UpdateTournament(Tournament tournament)
        {
            base.Insert(tournament);
        }
    }
}
