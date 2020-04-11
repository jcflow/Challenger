using System;
using GraphQL.Types;

namespace Schema.Types
{
    public class TeamInputType : InputObjectGraphType
    {
        public TeamInputType()
        {
            Name = "teamInput";
            Field<NonNullGraphType<StringGraphType>>("name");
        }
    }
}
