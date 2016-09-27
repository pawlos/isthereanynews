namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;
    using Services;
    using SharedData;
    using ViewModels;
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

            viewmodel.Providers = providers;

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

        public ActionResult Success()
        {
            this.loginService.RegisterIfNewUser();
            this.loginService.StoreCurrentUserIdInSession();
            this.loginService.AssignToUserRole();
            this.loginService.StoreItanRolesToSession();
            var viewmodel = new LoginSuccessViewModel();
            return this.View("Success", viewmodel);
        }
    }
}