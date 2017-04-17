namespace IsThereAnyNews.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels.Login;
    using IsThereAnyNews.Web.Interfaces.Services;
    using IsThereAnyNews.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    public partial class LoginController: Controller
    {
        private readonly IService service;
        private IAuthenticationManager AuthenticationManager => this.HttpContext.GetOwinContext().Authentication;

        public LoginController(IService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            var viewmodel = new AuthorizationIndexViewModel();
            var providers = this.HttpContext.GetOwinContext()
                .Authentication.GetAuthenticationTypes(x => !string.IsNullOrWhiteSpace(x.Caption))
                .ToList();
            var currentRegistrationStatus = this.service.GetCurrentRegistrationStatus();
            viewmodel.Providers = providers;
            viewmodel.CurrentRegistrationStatus = currentRegistrationStatus.ToString();
            return this.View("Index", viewmodel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, this.Url.Action("ExternalLoginCallback", "Login", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if(loginInfo == null)
            {
                return this.RedirectToAction(MVC.Login.Index());
            }

            var identity = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie);
            var currentRegistrationStatus = this.service.GetCurrentRegistrationStatus();
            switch(currentRegistrationStatus)
            {
                case RegistrationSupported.Closed:
                if(this.service.IsUserRegistered(identity) == false)
                {
                    return this.RedirectToAction("RegistrationClosed");
                }

                break;
                case RegistrationSupported.Open:
                // nothing here, proceed with regular registration
                break;
                case RegistrationSupported.InviteOnly:
                // not supported yet
                break;
                case RegistrationSupported.Limited:
                if(this.service.IsUserRegistered(identity) == false
                    && this.service.CanRegisterIfWithinLimits() == false)
                {
                    return this.RedirectToAction("RegistrationClosed");
                }

                break;
                default:
                throw new ArgumentOutOfRangeException();
            }

            this.service.RegisterIfNewUser(identity);
            this.service.StoreCurrentUserIdInSession(identity);
            this.service.StoreItanRolesToSession(identity);
            this.AuthenticationManager.SignIn(identity);
            return this.RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public virtual ActionResult RegistrationClosed()
        {
            this.Session.Clear();
            this.Request.GetOwinContext()
                .Authentication.SignOut(
                    this.HttpContext.GetOwinContext()
                        .Authentication.GetAuthenticationTypes()
                        .Select(o => o.AuthenticationType)
                        .ToArray());
            return this.View("RegistrationClosed");
        }

        [AllowAnonymous]
        public virtual ActionResult ExternalLoginFailure()
        {
            return this.View();
        }
    }
}