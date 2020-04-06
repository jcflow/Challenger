using System;
using System.Collections.Generic;
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

        public IEnumerable<Tournament> GetTournaments()
        {
            return base.Get();
        }

        public void InsertTournament(Tournament tournament)
        {
            base.Insert(tournament);
        }

        public void UpdateTournament(Tournament tournament)
        {
            base.Insert(tournament);
        }
    }
}
