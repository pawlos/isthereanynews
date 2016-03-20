using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ILoginService loginService;
        protected IUserAuthentication authentication;

        protected BaseController(IUserAuthentication authentication, ILoginService loginService)
        {
            this.authentication = authentication;
            this.loginService = loginService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session.IsNewSession)
            {
                var claimsPrincipal = this.authentication.GetCurrentUser();
                if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
                {
                    this.loginService.StoreCurrentUserIdInSession();
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}