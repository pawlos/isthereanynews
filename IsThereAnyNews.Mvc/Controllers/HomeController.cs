using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return this.View("Index");
        }

        public HomeController(
            IUserAuthentication authentication,
            ILoginService loginService)
            : base(authentication, loginService)
        {
        }
    }

    public class BaseController : Controller
    {
        protected ILoginService loginService;
        protected IUserAuthentication authentication;

        public BaseController(IUserAuthentication authentication, ILoginService loginService)
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