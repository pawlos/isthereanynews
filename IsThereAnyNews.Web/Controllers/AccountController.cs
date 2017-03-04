namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IService accountService;

        public AccountController(IService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public ActionResult ChangeDisplayName()
        {
            var viewmodel = this.accountService.GetAccountDetailsViewModel();
            return this.View("ChangeDisplayName", viewmodel);
        }

        [HttpPost]
        public ActionResult ChangeDisplayName(ChangeDisplayNameModelDto model)
        {
            if (this.ModelState.IsValid)
            {
                this.accountService.ChangeDisplayName(model);
                return this.RedirectToAction("Index");
            }

            var viewmodel = this.accountService.GetAccountDetailsViewModel();
            return this.View("ChangeDisplayName", viewmodel);
        }

        [HttpGet]
        public ActionResult ChangeEmail()
        {
            var viewmodel = this.accountService.GetAccountDetailsViewModel();
            return this.View("ChangeEmail", viewmodel);
        }

        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailModelDto model)
        {
            if (this.ModelState.IsValid)
            {
                this.accountService.ChangeEmail(model);
                return this.RedirectToAction("Index");
            }

            var viewmodel = this.accountService.GetAccountDetailsViewModel();
            return this.View("ChangeEmail", viewmodel);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewmodel = this.accountService.GetAccountDetailsViewModel();
            return this.View("Index", viewmodel);
        }
    }
}