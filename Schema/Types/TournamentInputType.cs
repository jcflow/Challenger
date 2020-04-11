using System;
using GraphQL.Types;

namespace Schema.Types
{
    public class TournamentInputType : InputObjectGraphType
    {
        public TournamentInputType()
        {
            Name = "tournamentInput";
            Field<NonNullGraphType<StringGraphType>>("name");
        }
    }
}
