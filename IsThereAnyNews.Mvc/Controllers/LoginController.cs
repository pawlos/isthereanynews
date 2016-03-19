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
    public class LoginController : BaseController
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

        public ActionResult SocialLogin(AuthenticationTypeProvider id)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "/Login/Success",
                IsPersistent = true
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

        public LoginController(IUserAuthentication authentication, ILoginService loginService) : base(authentication, loginService)
        {
        }
    }

}