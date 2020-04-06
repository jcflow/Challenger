using System;
using System.Linq;
using GraphQL.Types;
using Models;
using Repository;

namespace Schema.Types
{
    public class ScoreType : ObjectGraphType<Score>
    {
        public ScoreType(ITeamRepository teamRepository)
        {
            Name = "Score";
            Description = "Represents a score of a team in a bracket in a tournament.";

            Field(_ => _.ID).Name("id").Description("The score ID.");
            Field(_ => _.Value).Name("value").Description("The score value.");

            Field<TeamType>(
                "team",
                Description = "The score's team.",
                resolve: context =>
                {
                    var parent = context.Source;
                    return teamRepository.GetTeamByID(parent.TeamID);
                });
        }
    }
}
