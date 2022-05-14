using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using AppFramework.Queries.Container;
using System;

namespace AppFramework.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}