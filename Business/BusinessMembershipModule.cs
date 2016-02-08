using System.Reflection;
using Business.Impl;
using Autofac;

namespace Business
{
    public class BusinessMembershipModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(MemberBusiness).Assembly)
                .Where(t => t.Name.EndsWith("Business"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
  
        }
    }
}
