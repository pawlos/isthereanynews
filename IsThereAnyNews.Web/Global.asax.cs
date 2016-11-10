﻿namespace IsThereAnyNews.Web
{
    using System.Security.Claims;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using IsThereAnyNews.Autofac;
    using IsThereAnyNews.RssChannelUpdater;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IsThereAnyNewsAutofac.RegisterDependencies();
            IsThereAnyNewsScheduler.ScheduleRssUpdater();
        }
    }
}
