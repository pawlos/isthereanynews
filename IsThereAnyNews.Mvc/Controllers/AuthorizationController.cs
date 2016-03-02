﻿using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;
    using ViewModels.Login;

    [AllowAnonymous]
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            var viewmodel = new AuthorizationIndexViewModel();
            var providers = HttpContext.GetOwinContext()
                                       .Authentication
                                       .GetAuthenticationTypes(x => !string.IsNullOrWhiteSpace(x.Caption))
                                       .ToList();

            viewmodel.Providers = providers;

            return this.View("Index", viewmodel);
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(ConstantStrings.AuthorizationCookieName);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SocialLogin(string id)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "/home"
            };
            HttpContext.GetOwinContext().Authentication.Challenge(authenticationProperties, id);
            return new HttpUnauthorizedResult();
        }
    }
}