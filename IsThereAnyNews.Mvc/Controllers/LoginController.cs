namespace IsThereAnyNews.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security;

    using Services;

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
            var providers = this.HttpContext.GetOwinContext()
                                       .Authentication
                                       .GetAuthenticationTypes(x => !string.IsNullOrWhiteSpace(x.Caption))
                                       .ToList();

            var currentRegistrationStatus = this.loginService.GetCurrentRegistrationStatus();

            viewmodel.Providers = providers;
            viewmodel.CurrentRegistrationStatus = currentRegistrationStatus.ToString();

            return this.View("Index", viewmodel);
        }

        public ActionResult SocialLogin(AuthenticationTypeProvider id)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "/Login/Success",
                IsPersistent = true
            };

            this.HttpContext.GetOwinContext().Authentication.Challenge(authenticationProperties, id.ToString());
            return new HttpUnauthorizedResult();
        }

        public async Task<ActionResult> Success()
        {
            var claimsIdentity = await this.GetExtStatus();

            if (claimsIdentity == null)
            {
                return this.RedirectToAction("Index");
            }

            var currentRegistrationStatus = this.loginService.GetCurrentRegistrationStatus();
            switch (currentRegistrationStatus)
            {
                case RegistrationSupported.Closed:
                    if (this.loginService.IsUserRegistered() == false)
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
                    if (this.loginService.IsUserRegistered() == false &&
                        this.loginService.CanRegisterIfWithinLimits() == false)
                    {
                        return this.RedirectToAction("RegistrationClosed");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.loginService.RegisterIfNewUser();
            this.loginService.StoreCurrentUserIdInSession();
            this.loginService.StoreItanRolesToSession();
            return this.RedirectToAction("Index", "Home");
        }

        private async Task<ClaimsIdentity> GetExtStatus()
        {
            return await this.HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(ConstantStrings.AuthorizationCookieName);
        }

        [HttpGet]
        public ActionResult RegistrationClosed()
        {
            this.Session.Clear();
            Request.GetOwinContext()
                .Authentication.SignOut(
                    HttpContext.GetOwinContext()
                        .Authentication.GetAuthenticationTypes()
                        .Select(o => o.AuthenticationType)
                        .ToArray());
            return this.View("RegistrationClosed");
        }
    }
}