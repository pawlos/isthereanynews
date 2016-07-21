namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;

    public class ContactController : Controller
    {
        private readonly IContactService contactService;

        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        public ActionResult Index()
        {
            var viewModel = this.contactService.GetViewModel();
            return this.View("Index", viewModel);
        }

        [HttpPost]
        public ActionResult ContactAdministration(ContactAdministrationModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.contactService.SaveAdministrationContact(model);
                return this.RedirectToAction("Index");
            }

            var contactViewModel = this.contactService.GetViewModel();
            return this.View("Index", contactViewModel);
        }
    }
}