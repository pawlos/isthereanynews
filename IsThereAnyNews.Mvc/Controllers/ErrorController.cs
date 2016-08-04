namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        [PreventDirectAccess]
        public ActionResult ServerError()
        {
            return this.View("Execute");
        }

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

        private class PreventDirectAccessAttribute : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                object value = filterContext.RouteData.Values["fromAppErrorEvent"];
                if (!(value is bool && (bool)value))
                {
                    filterContext.Result = new ViewResult { ViewName = "Error404" };
                }
            }
        }
    }
}