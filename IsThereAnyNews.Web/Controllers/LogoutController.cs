using Microsoft.AspNet.Identity;

namespace IsThereAnyNews.Web.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using IsThereAnyNews.SharedData;

    public partial class LogoutController: Controller
    {
        public virtual ActionResult Index()
        {
            this.Session.Abandon();
            this.Session.Clear();
            this.HttpContext
                .GetOwinContext()
                .Authentication
                .SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction(MVC.Home.Index());
        }
    }
}