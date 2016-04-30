using System.Web.Mvc;
using System.Web.Routing;

namespace IsThereAnyNews.Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Stream",
                url: "{controller}/{action}/{streamtype}/{id}",
                defaults: new {controller = "Stream", action = "Read" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
                );

        }
    }
}
