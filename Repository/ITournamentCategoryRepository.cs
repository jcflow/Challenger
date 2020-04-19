using System;
using System.Collections.Generic;
using Models;

namespace Repository
{
    public interface ITournamentCategoryRepository
    {
        IEnumerable<TournamentCategory> GetTournamentCategories();
        TournamentCategory GetTournamentCategoryByID(int tournamentCategoryId);
        TournamentCategory InsertTournamentCategory(TournamentCategory tournamentCategory);
        void DeleteTournamentCategory(int tournamentCategoryId);
        void UpdateTournamentCategory(TournamentCategory tournamentCategory);
    }
}
