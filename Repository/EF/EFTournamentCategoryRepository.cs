using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository.EF
{
    public class EFTournamentCategoryRepository : EFRepository<TournamentCategory>, ITournamentCategoryRepository
    {
        public EFTournamentCategoryRepository(ChallengerContext challengerContext) : base(challengerContext)
        {
        }

        public void DeleteTournamentCategory(int tournamentCategoryId)
        {
            base.Delete(tournamentCategoryId);
        }

        public IEnumerable<TournamentCategory> GetTournamentCategories()
        {
            return base.Get();
        }

        public TournamentCategory GetTournamentCategoryByID(int tournamentCategoryId)
        {
            return base.GetByID(tournamentCategoryId);
        }

        public TournamentCategory InsertTournamentCategory(TournamentCategory tournamentCategory)
        {
            return base.Insert(tournamentCategory);
        }

        public void UpdateTournamentCategory(TournamentCategory tournamentCategory)
        {
            base.Update(tournamentCategory);
        }
    }
}
