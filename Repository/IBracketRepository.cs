using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository
{
    public interface IBracketRepository
    {
        IEnumerable<Bracket> GetBrackets(
               Expression<Func<Bracket, bool>> filter = null,
               Func<IQueryable<Bracket>, IOrderedQueryable<Bracket>> orderBy = null);
        Bracket GetBracketByID(int bracketId);
        void InsertBracket(Bracket bracket);
        void DeleteBracket(int bracketId);
        void UpdateBracket(Bracket bracket);
    }
}
