namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        [PreventDirectAccess]
        public ActionResult AccessDenied()
        {
            return this.View("Execute");
        }

        public ActionResult NotFound()
        {
            return this.View("Execute");
        }

        [PreventDirectAccess]
        public ActionResult OtherHttpStatusCode(int httpStatusCode)
        {
            return this.View("Execute", httpStatusCode);
        }

        [PreventDirectAccess]
        public ActionResult ServerError()
        {
            return this.View("Execute");
        }

        private class PreventDirectAccessAttribute : FilterAttribute,
                                                     IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                var value = filterContext.RouteData.Values["fromAppErrorEvent"];
                if (!(value is bool && (bool)value))
                {
                    filterContext.Result = new ViewResult { ViewName = "Error404" };
                }
            }
        }
    }
}