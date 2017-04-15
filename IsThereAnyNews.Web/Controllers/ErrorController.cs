namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    public partial class ErrorController: Controller
    {
        [PreventDirectAccess]
        public virtual ActionResult AccessDenied()
        {
            return this.View("Execute");
        }

        public virtual ActionResult NotFound()
        {
            return this.View("Execute");
        }

        [PreventDirectAccess]
        public virtual ActionResult OtherHttpStatusCode(int httpStatusCode)
        {
            return this.View("Execute", httpStatusCode);
        }

        [PreventDirectAccess]
        public virtual ActionResult ServerError()
        {
            return this.View("Execute");
        }

        private class PreventDirectAccessAttribute: FilterAttribute,
                                                     IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                var value = filterContext.RouteData.Values["fromAppErrorEvent"];
                if(!(value is bool && (bool)value))
                {
                    filterContext.Result = new ViewResult { ViewName = "Error404" };
                }
            }
        }
    }
}