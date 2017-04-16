namespace IsThereAnyNews.Web.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    public partial class LogoutController: Controller
    {
        private IAuthenticationManager AuthenticationManager => this.HttpContext.GetOwinContext().Authentication;

        public virtual ActionResult Index()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Index", "Home");
        }
    }
}