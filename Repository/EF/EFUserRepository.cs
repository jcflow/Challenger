using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository.EF
{
    public class EFUserRepository : EFRepository<User>, IUserRepository
    {
        public EFUserRepository(ChallengerContext challengerContext) : base(challengerContext)
        {
        }

        public void DeleteUser(int userId)
        {
            base.Delete(userId);
        }

        public User GetUserByID(int userId)
        {
            return base.GetByID(userId);
        }

        public IEnumerable<User> GetUsers(Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null)
        {
            return base.GetAsync(filter, orderBy);
        }

        public User InsertUser(User user)
        {
            return base.Insert(user);
        }

        public void UpdateUser(User user)
        {
            base.Update(user);
        }
    }
}
