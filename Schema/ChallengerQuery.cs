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
        }
    }
}
