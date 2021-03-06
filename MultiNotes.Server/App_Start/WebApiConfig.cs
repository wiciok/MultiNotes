﻿using System.Web.Http;

namespace MultiNotes.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                 //routeTemplate: "api/{controller}/{id}/{specificID}",
                 //defaults: new { id = RouteParameter.Optional, specificID=RouteParameter.Optional }
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id=RouteParameter.Optional }
            );
        }
    }
}
