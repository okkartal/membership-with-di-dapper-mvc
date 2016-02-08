using System.Web;
using Autofac;
using Autofac.Core;
using Infrastructure.Security;
using Infrastructure.WebContext;

namespace Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Hash Provider
            builder.RegisterType<SaltedHash>().As<IHashProvider>().InstancePerLifetimeScope();
             
            builder.RegisterType<CookieManager>().As<ICookieManager>()
                .WithParameter(new ResolvedParameter((info, context) => info.ParameterType == typeof(HttpContext), (info, context) => HttpContext.Current))
                .InstancePerLifetimeScope();
          }
    }
}
