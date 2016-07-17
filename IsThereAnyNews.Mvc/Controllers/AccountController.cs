namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Services;

    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;

        public AccountController(
            IUserAuthentication authentication,
            ILoginService loginService,
            ISessionProvider sessionProvider,
            IAccountService accountService)
            : base(authentication, loginService, sessionProvider)
        {
            this.accountService = accountService;
        }

        public ActionResult Index()
        {
            var viewmodel = this.accountService.GetAccountDetailsViewModel();
            return this.View("Index", viewmodel);
        }
    }
}