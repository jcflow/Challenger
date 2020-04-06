using System;
using System.Collections.Generic;
using Models;

namespace Repository
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> GetTournaments();
        Tournament GetTournamentByID(int tournamentId);
        void InsertTournament(Tournament tournament);
        void DeleteTournament(int tournamentId);
        void UpdateTournament(Tournament tournament);
    }
}
