    using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> GetTournaments(
               Expression<Func<Tournament, bool>> filter = null,
               Func<IQueryable<Tournament>, IOrderedQueryable<Tournament>> orderBy = null);
        Tournament GetTournamentByID(int tournamentId);
        Tournament InsertTournament(Tournament tournament);
        void DeleteTournament(int tournamentId);
        void UpdateTournament(Tournament tournament);
    }
}
