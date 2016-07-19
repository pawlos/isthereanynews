namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

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
    }
}