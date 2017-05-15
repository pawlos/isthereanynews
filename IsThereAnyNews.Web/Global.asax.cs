namespace IsThereAnyNews.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Exceptionless;

    using IsThereAnyNews.Autofac;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.RssChannelUpdater;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Controllers;

    public class MvcApplication: HttpApplication
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
#if !DEBUG
        private IEnumerable<Exception> GetAllExceptions(Exception ex)
        {
            var currentEx = ex;
            yield return currentEx;
            while(currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx;
            }
        }

        private void SaveExceptionToDatabase(Exception httpException)
        {
            var exceptionGuid = Guid.NewGuid();

            var repository = DependencyResolver.Current.GetService(typeof(IEntityRepository)) as IEntityRepository;
            var claimsPrincipal = this.User as ClaimsPrincipal;
            var userId = long.Parse(
                    claimsPrincipal?
                    .Claims?
                    .SingleOrDefault(x => x.Type == ItanClaimTypes.ApplicationIdentifier)?
                    .Value ?? "0");

            var exceptions = this.GetAllExceptions(httpException)
                                 .ToList()
                                 .Select(
                                 exception => new ItanException
                                              {
                                                  Message = exception.Message,
                                                  Source = exception.Source,
                                                  Stacktrace = exception.StackTrace,
                                                  Typeof = exception.GetType()
                                                                    .ToString(),
                                                  ErrorId = exceptionGuid,
                                                  UserId = userId
                                              });
            var eventItanExceptions =
                    exceptions.Select(e => new EventItanException {ErrorId = exceptionGuid, ItanException = e});

            repository.SaveExceptionToDatabase(eventItanExceptions);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            this.ShowCustomErrorPage(this.Server.GetLastError());
        }

        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException ?? new HttpException(500, "Internal Server Error", exception);
            exception.ToExceptionless().Submit();
            this.SaveExceptionToDatabase(httpException);
            this.Response.Clear();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);
            switch(httpException.GetHttpCode())
            {
                case 403:
                routeData.Values.Add("action", "AccessDenied");
                break;
                case 404:
                routeData.Values.Add("action", "NotFound");
                break;
                case 500:
                routeData.Values.Add("action", "ServerError");
                break;
                default:
                routeData.Values.Add("action", "OtherHttpStatusCode");
                routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                break;
            }

            this.Server.ClearError();
            this.Response.TrySkipIisCustomErrors = true;
            this.Response.Headers.Add("Content-Type", "text/html");
            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(this.Context), routeData));
        }
#endif
    }
}