using GraphQL.Types;
using Schema.Types;
using Repository;

namespace Schema
{
    public class ChallengerQuery : ObjectGraphType
    {
        public ChallengerQuery(ITournamentRepository tournamentRepository, ITournamentCategoryRepository tournamentCategoryRepository)
        {
            Field<TournamentCategoryType>(
                "category",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return tournamentCategoryRepository.GetTournamentCategoryByID(id);
                }
            );

            Field<ListGraphType<TournamentCategoryType>>(
                "categories",
                Description = "All tournament categories.",
                resolve: context =>
                {
                    return tournamentCategoryRepository.GetTournamentCategories();
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
