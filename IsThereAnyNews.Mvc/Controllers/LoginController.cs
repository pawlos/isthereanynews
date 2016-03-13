﻿using IsThereAnyNews.Services;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;
    using SharedData;
    using ViewModels.Login;

    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

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

        public ActionResult SocialLogin(AuthenticationTypeProvider id)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "/Login/Success"
            };
            HttpContext.GetOwinContext().Authentication.Challenge(authenticationProperties, id.ToString());
            return new HttpUnauthorizedResult();
        }

        public ActionResult Success()
        {
            this.loginService.RegisterIfNewUser();
            this.loginService.StoreCurrentUserIdInSession();
            var viewmodel = new LoginSuccessViewModel();
            return this.View("Success", viewmodel);
        }
    }

}