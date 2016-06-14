using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILoginService loginService;
        protected readonly IUserAuthentication authentication;
        private readonly ISessionProvider sessionProvider;

        protected BaseController(IUserAuthentication authentication, ILoginService loginService, ISessionProvider sessionProvider)
        {
            this.authentication = authentication;
            this.loginService = loginService;
            this.sessionProvider = sessionProvider;
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            var claims = this.sessionProvider.LoadClaims();
            if (claims != null)
            {
                var claimsIdentity = this.User as ClaimsPrincipal;
                var itanIdentity = new ClaimsIdentity(claims, "ITAN");
                claimsIdentity.AddIdentity(itanIdentity);
            }
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