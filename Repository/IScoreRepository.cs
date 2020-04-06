using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository
{
    public interface IScoreRepository
    {
        IEnumerable<Score> GetScores(
               Expression<Func<Score, bool>> filter = null,
               Func<IQueryable<Score>, IOrderedQueryable<Score>> orderBy = null);
        Score GetScoreByID(int scoreId);
        void InsertScore(Score score);
        void DeleteScore(int scoreId);
        void UpdateScore(Score score);
    }
}
