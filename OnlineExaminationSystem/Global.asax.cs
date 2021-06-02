using OnlineExaminationSystem.App_Start;
using OnlineExaminationSystem.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineExaminationSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        OnlineExaminationDatabaseEntities context = new OnlineExaminationDatabaseEntities();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;
            log4net.Config.XmlConfigurator.Configure();
            var exists = context.Database.Exists();
            if (!exists)
            {
                context.Database.CreateIfNotExists();
                Seed.DatabaseSeed(context);
            }
        }
     
    }
}
