using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers(
               Expression<Func<User, bool>> filter = null,
               Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null);
        User GetUserByID(int userId);
        User InsertUser(User user);
        void DeleteUser(int userId);
        void UpdateUser(User user);
    }
}
