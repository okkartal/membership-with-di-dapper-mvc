using System.Linq;
using Autofac; 
using Autofac.Core;
using IoC;

namespace Dapper
{
    public class DataDapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DataDapperModule).Assembly)
                .As(t => t.GetInterfaces()
                    .Where(i => i.Name.EndsWith("Repository"))
                    .Select(i => new KeyedService(RepositoryType.Dapper, i)))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(DataDapperModule).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
