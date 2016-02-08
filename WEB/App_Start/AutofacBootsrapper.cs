using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Business;
using Infrastructure;
using IoC;
using Dapper;
using Autofac.Integration.Mvc;

namespace WEB.App_Start
{
    public class AutofacBootsrapper
    {
        public static void Register()
        {
            Bootstrapper.ScopeProvider = new MvcScopeProvider();
            Bootstrapper.InitializeBuilder();

            Bootstrapper.Builder.RegisterModule<InfrastructureModule>(); 
            Bootstrapper.Builder.RegisterModule<DataDapperModule>();   
            Bootstrapper.Builder.RegisterModule<BusinessMembershipModule>();
            Bootstrapper.Builder.Register<IDbConnection>(c => new SqlConnection(ConfigurationManager.ConnectionStrings["DBPath"].ConnectionString));
            Bootstrapper.Builder.Register<DbConnectionFactory>(c => new DbConnectionFactory
            {
                DbPath = c.Resolve<IDbConnection>()
            });
            //For MVC
            Bootstrapper.Builder.RegisterControllers(Assembly.GetExecutingAssembly());
            Bootstrapper.Builder.RegisterFilterProvider();

            Bootstrapper.SetAutofacContainer();
     
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Bootstrapper.Container));
        }
    }
}