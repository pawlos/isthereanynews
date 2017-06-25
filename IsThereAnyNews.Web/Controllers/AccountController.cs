namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Web.Interfaces.Services;

    [Authorize]
    public partial class AccountController : Controller
    {
        private readonly IService service;

        public AccountController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("Index", viewmodel);
        }

        [HttpGet]
        public virtual ActionResult ChangeDisplayName()
        {
            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeDisplayName", viewmodel);
        }

        [HttpPost]
        public virtual ActionResult ChangeDisplayName(ChangeDisplayNameModelDto model)
        {
            if(this.ModelState.IsValid)
            {
                this.service.ChangeDisplayName(model);
                return this.RedirectToAction("Index");
            }

            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeDisplayName", viewmodel);
        }

        [HttpGet]
        public virtual ActionResult ChangeEmail()
        {
            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeEmail", viewmodel);
        }

        [HttpPost]
        public virtual ActionResult ChangeEmail(ChangeEmailModelDto model)
        {
            if(this.ModelState.IsValid)
            {
                this.service.ChangeEmail(model);
                return this.RedirectToAction("Index");
            }

            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeEmail", viewmodel);
        }
    }
}