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
}