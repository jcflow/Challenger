using System;
using System.Collections.Generic;
using GraphQL.Types;
using Models;
using Repository;

namespace Schema.Types
{
    public class TournamentType : ObjectGraphType<Tournament>
    {
        public TournamentType(ITournamentCategoryRepository tournamentCategoryRepository, IBracketRepository bracketRepository)
        {
            Name = "Tournament";
            Description = "Represents a tournament.";

            Field(_ => _.ID).Name("id").Description("The tournament ID.");
            Field(_ => _.Name).Name("name").Description("The tournament name.");

            Field<TournamentCategoryType>(
                "category",
                Description = "The tournament's category.",
                resolve: context => {
                    var parent = context.Source;
                    return tournamentCategoryRepository.GetTournamentCategoryByID(parent.CategoryID);
                });

            Field<ListGraphType<BracketType>>(
                "brackets",
                Description = "Kategoriye ait urunler",
                resolve: context => {
                    var parent = context.Source;
                    return bracketRepository.GetBrackets(_ => _.TournamentID == parent.ID);
                });
        }
    }
}
