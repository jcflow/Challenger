using System;
using GraphQL.Types;
using Models;
using Repository;

namespace Schema.Types
{
    public class BracketType : ObjectGraphType<Bracket>
    {
        public BracketType(ITournamentRepository tournamentRepository, IScoreRepository scoreRepository)
        {
            Name = "Bracket";
            Description = "Represents a bracket in a tournament.";

            Field(_ => _.ID).Name("id").Description("The bracket ID.");
            Field(_ => _.Finished).Name("finished").Description("The status of the bracket.");
            Field(_ => _.Level).Name("level").Description("The level of the bracket in the tournament.");

            Field<ListGraphType<ScoreType>>(
                "scores",
                Description = "The bracket's scores.",
                resolve: context =>
                {
                    var parent = context.Source;
                    return scoreRepository.GetScores(_ => _.BracketID == parent.ID);
                });
        }
    }
}
