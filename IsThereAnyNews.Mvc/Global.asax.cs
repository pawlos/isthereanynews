using IsThereAnyNews.Autofac;
using IsThereAnyNews.Automapper;
using IsThereAnyNews.RssChannelUpdater;

namespace IsThereAnyNews.Mvc
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            IsThereAnyNewsAutofac.RegisterDependencies();
            IsThereAnyNewsScheduler.ScheduleRssUpdater();
            IsThereAnyNewsMapper.RegisterMappings();
        }
    }
}
