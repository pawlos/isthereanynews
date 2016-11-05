namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;

    using IsThereAnyNews.Services;

    public abstract class BaseController : Controller
    {
        protected readonly ILoginService loginService;
        protected readonly IUserAuthentication authentication;

        protected BaseController(IUserAuthentication authentication, ILoginService loginService)
        {
            this.authentication = authentication;
            this.loginService = loginService;
        }

        //protected override void OnAuthentication(AuthenticationContext filterContext)
        //{
        //    if (this.Session.IsNewSession)
        //    {
        //        this.RestoreUserCredentials();
        //    }

        //    var claims = this.sessionProvider.LoadClaims();
        //    if (claims == null)
        //    {
        //        return;
        //    }

        //    var claimsIdentity = this.User as ClaimsPrincipal;
        //    if (claimsIdentity.Identities.Any(i => i.AuthenticationType == "ITAN"))
        //    {
        //        return;
        //    }

        //    var itanIdentity = new ClaimsIdentity(claims, "ITAN");
        //    claimsIdentity.AddIdentity(itanIdentity);
        //}

        //private void RestoreUserCredentials()
        //{
        //    var claimsPrincipal = this.authentication.GetCurrentUser();
        //    if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
        //    {
        //        this.loginService.StoreCurrentUserIdInSession();
        //        this.loginService.StoreItanRolesToSession();
        //    }
        //}
    }
}