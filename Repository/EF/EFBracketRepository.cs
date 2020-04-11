using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository.EF
{
    public class EFBracketRepository : EFRepository<Bracket>, IBracketRepository
    {
        public EFBracketRepository(ChallengerContext challengerContext) : base(challengerContext)
        {
        }

        public void DeleteBracket(int bracketId)
        {
            base.Delete(bracketId);
        }

        public Bracket GetBracketByID(int bracketId)
        {
            return base.GetByID(bracketId);
        }

        public IEnumerable<Bracket> GetBrackets(Expression<Func<Bracket, bool>> filter = null,
            Func<IQueryable<Bracket>, IOrderedQueryable<Bracket>> orderBy = null)
        {
            return base.Get(filter, orderBy);
        }

        public Bracket InsertBracket(Bracket bracket)
        {
            return base.Insert(bracket);
        }

        public void UpdateBracket(Bracket bracket)
        {
            base.Update(bracket);
        }
    }
}
