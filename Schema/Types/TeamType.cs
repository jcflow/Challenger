using System;
using GraphQL.Types;
using Models;

namespace Schema.Types
{
    public class TeamType : ObjectGraphType<Team>
    {
        public TeamType()
        {
            Name = "Team";
            Description = "Represents a team in a tournament.";

            Field(_ => _.ID).Name("id").Description("The team ID.");
            Field(_ => _.Name).Name("name").Description("The team name.");
        }
    }
}
