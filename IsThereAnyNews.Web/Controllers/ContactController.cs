namespace IsThereAnyNews.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Web.Interfaces.Services;

    public partial class ContactController: Controller
    {
        private readonly IService service;

        public ContactController(IService service)
        {
            this.service = service;
        }

        public virtual ActionResult Index()
        {
            var viewModel = this.service.GetViewModel();
            return this.View("Index", viewModel);
        }

        [HttpPost]
        public virtual ActionResult Index(ContactAdministrationDto dto)
        {
            if(this.ModelState.IsValid)
            {
                this.service.SaveAdministrationContact(dto);
                return this.RedirectToAction(MVC.Contact.Success());
            }

            var contactViewModel = this.service.GetViewModel();
            return this.View("Index", contactViewModel);
        }

        [HttpGet]
        public virtual ActionResult Success()
        {
            return this.View("Success");
        }
    }
}