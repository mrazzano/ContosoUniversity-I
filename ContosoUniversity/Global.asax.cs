using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using System.Web.Optimization;
using System.Data.Entity.Infrastructure.Interception;
using ContosoUniversity.ActionFilters;
using ContosoUniversity.Infrastructure.Database;

namespace ContosoUniversity
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            GlobalFilters.Filters.Add(new WhitespaceFilterAttribute());

            var isDatabaseLoggingEnabled = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableDatabaseLogging"]) 
                && Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDatabaseLogging"]);

            if(isDatabaseLoggingEnabled)
                DbInterception.Add(new SchoolInterceptorLogging());
        }
    }
}
