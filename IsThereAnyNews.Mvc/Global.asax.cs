namespace IsThereAnyNews.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using IsThereAnyNews.Autofac;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.Mvc.Controllers;
    using IsThereAnyNews.RssChannelUpdater;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            IsThereAnyNewsAutofac.RegisterDependencies();
            IsThereAnyNewsScheduler.ScheduleRssUpdater();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            this.ShowCustomErrorPage(this.Server.GetLastError());
        }

        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException ?? new HttpException(500, "Internal Server Error", exception);

            this.SaveExceptionToDatabase(httpException as Exception);

            this.Response.Clear();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
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

        private void SaveExceptionToDatabase(Exception httpException)
        {
            var exceptionGuid = Guid.NewGuid();
            IExceptionRepository repository =
                DependencyResolver.Current.GetService(typeof(IExceptionRepository)) as IExceptionRepository;
            var exceptions = this.GetAllExceptions(httpException)
                .ToList()
                .Select(
                    exception =>
                    new ItanException
                    {
                        Message = exception.Message,
                        Source = exception.Source,
                        Stacktrace = exception.StackTrace,
                        Typeof = exception.GetType().ToString(),
                        ErrorId = exceptionGuid
                    });

            repository.SaveToDatabase(exceptions);
        }

        private IEnumerable<Exception> GetAllExceptions(Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx;
            }
        }
    }
}
