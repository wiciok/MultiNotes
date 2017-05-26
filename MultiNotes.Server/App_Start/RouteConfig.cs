using System.Web.Mvc;
using System.Web.Routing;

namespace MultiNotes.Server
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //kolejność ma znaczenie! jeśli dodamy nową regułę, a będzie się ona łapać pod domyślną, to MUSI być ona wyżej, bo inaczej nie będzie działać!

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("ResetPassword", "ResetPassword/Reset/{token}", new { controller = "ResetPassword", action = "Reset", token = "undefinied" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional } 
            );


        }
    }
}
