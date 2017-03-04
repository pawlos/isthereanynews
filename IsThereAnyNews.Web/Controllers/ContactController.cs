namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;

    public class ContactController : Controller
    {
        private readonly IService service;

        public ContactController(IService service)
        {
            this.service = service;
        }

        [HttpPost]
        public ActionResult ContactAdministration(ContactAdministrationDto dto)
        {
            if (this.ModelState.IsValid)
            {
                this.service.SaveAdministrationContact(dto);
                return this.RedirectToAction("Success");
            }

            var contactViewModel = this.service.GetViewModel();
            return this.View("Index", contactViewModel);
        }

        public ActionResult Index()
        {
            var viewModel = this.service.GetViewModel();
            return this.View("Index", viewModel);
        }

        [HttpGet]
        public ActionResult Success()
        {
            return this.View("Success");
        }
    }
}