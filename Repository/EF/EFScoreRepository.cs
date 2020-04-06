using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository.EF
{
    public class EFScoreRepository : EFRepository<Score>, IScoreRepository
    {
        public EFScoreRepository(ChallengerContext challengerContext) : base(challengerContext)
        {
        }

        public void DeleteScore(int scoreId)
        {
            base.Delete(scoreId);
        }

        public Score GetScoreByID(int scoreId)
        {
            return base.GetByID(scoreId);
        }

        public IEnumerable<Score> GetScores(Expression<Func<Score, bool>> filter = null,
            Func<IQueryable<Score>, IOrderedQueryable<Score>> orderBy = null)
        {
            return base.Get(filter, orderBy);
        }

        public void InsertScore(Score score)
        {
            base.Insert(score);
        }

        public void UpdateScore(Score score)
        {
            base.Update(score);
        }
    }
}
