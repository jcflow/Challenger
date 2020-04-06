using System;
using GraphQL;

namespace Schema
{
    public class ChallengerSchema : GraphQL.Types.Schema
    {
        public ChallengerSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<ChallengerQuery>();
        }
    }
}
