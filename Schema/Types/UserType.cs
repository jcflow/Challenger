using System;
using System.Collections.Generic;
using GraphQL.Types;
using Models;
using Repository;

namespace Schema.Types
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType(ITournamentRepository tournamentRepository)
        {
            Name = "User";
            Description = "Represents a user.";

            Field(_ => _.ID).Name("id").Description("The user ID.");
            Field(_ => _.Name).Name("name").Description("The user name.");
        }
    }
}
