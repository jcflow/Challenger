using System;
using System.Collections.Generic;
using GraphQL.Types;
using Models;
using Schema.Types;
using Repository;

namespace Schema
{
    public class ChallengerQuery : ObjectGraphType
    {
        public ChallengerQuery(ITournamentRepository tournamentRepository)
        {
            Field<ListGraphType<TournamentType>>(
                "tournaments",
                Description = "Tum Urunler",
                resolve: context =>
                {
                    return tournamentRepository.GetTournaments();
                }
            );

            Field<TournamentType>(
                "tournament",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return tournamentRepository.GetTournamentByID(id);
                }
            );
        }
    }
}
