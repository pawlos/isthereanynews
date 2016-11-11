namespace IsThereAnyNews.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels.Login;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    [Authorize]
    public class SocialController : Controller
    {
        private const string XsrfKey = "XsrfId";

        private readonly ILoginService loginService;

        public SocialController(ILoginService loginService)
        {
            this.loginService = loginService;
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var viewmodel = new AuthorizationIndexViewModel();
            var providers = this.HttpContext.GetOwinContext()
                                       .Authentication
                                       .GetAuthenticationTypes(x => !string.IsNullOrWhiteSpace(x.Caption))
                                       .ToList();

            var currentRegistrationStatus = this.loginService.GetCurrentRegistrationStatus();

            viewmodel.Providers = providers;
            viewmodel.CurrentRegistrationStatus = currentRegistrationStatus.ToString();
            return this.View("Login", viewmodel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(
                       provider,
                       this.Url.Action("ExternalLoginCallback", "Social", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return this.RedirectToAction("Login");
            }

            var identity = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie);

            var currentRegistrationStatus = this.loginService.GetCurrentRegistrationStatus();
            switch (currentRegistrationStatus)
            {
                case RegistrationSupported.Closed:
                    if (this.loginService.IsUserRegistered(identity) == false)
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
                    if (this.loginService.IsUserRegistered(identity) == false &&
                        this.loginService.CanRegisterIfWithinLimits() == false)
                    {
                        return this.RedirectToAction("RegistrationClosed");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.loginService.RegisterIfNewUser(identity);
            this.loginService.StoreCurrentUserIdInSession(identity);
            this.loginService.StoreItanRolesToSession(identity);
            this.AuthenticationManager.SignIn(identity);

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return this.View();
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

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                this.LoginProvider = provider;
                this.RedirectUri = redirectUri;
                this.UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
                if (this.UserId != null)
                {
                    properties.Dictionary[XsrfKey] = this.UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
            }
        }
    }
}