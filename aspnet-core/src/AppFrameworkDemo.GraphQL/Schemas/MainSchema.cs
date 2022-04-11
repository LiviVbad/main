using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using AppFrameworkDemo.Queries.Container;
using System;

namespace AppFrameworkDemo.Schemas
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