﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MultiNotes.Server.Services;
using NLog;

namespace MultiNotes.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Logger GlobalLogger = LogManager.GetLogger("MongoLogger");


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalFilters.Filters.Add(new LogHttpRequest());

            GlobalLogger.Info("Logging started");
        }
    }
}
