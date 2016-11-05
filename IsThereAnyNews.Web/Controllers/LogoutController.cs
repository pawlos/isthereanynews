namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web;
    using System.Web.Mvc;

    using IsThereAnyNews.SharedData;

    [Authorize]
    public class LogoutController : Controller
    {
        public ActionResult Index()
        {
            this.Session.Abandon();
            this.Session.Clear();
            this.HttpContext.GetOwinContext().Authentication.SignOut(ConstantStrings.AuthorizationCookieName);
            return this.RedirectToAction("Index", "Home");
        }
    }
}