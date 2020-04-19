using System;
using GraphQL.Types;
using Models;
using Repository;

namespace Schema.Types
{
    public class TournamentCategoryType : ObjectGraphType<TournamentCategory>
    {
        public TournamentCategoryType(ITournamentRepository tournamentRepository)
        {
            Name = "Tournament Category";
            Description = "Represents a tournament category.";

            Field(_ => _.ID).Name("id").Description("The category ID.");
            Field(_ => _.Name).Name("name").Description("The category name.");

            Field<ListGraphType<TournamentType>>(
                "tournaments",
                Description = "Tum Urunler",
                resolve: context =>
                {
                    var parent = context.Source;
                    return tournamentRepository.GetTournaments(_ => _.CategoryID == parent.ID);
                }
            );
        }
    }
}
