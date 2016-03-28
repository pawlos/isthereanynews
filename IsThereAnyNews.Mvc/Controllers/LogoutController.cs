using System.Web;
using System.Web.Mvc;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Mvc.Controllers
{
    [Authorize]
    public class LogoutController : Controller
    {
        public ActionResult Index()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(ConstantStrings.AuthorizationCookieName);
            return this.RedirectToAction("Index", "Home");
        }
    }
}