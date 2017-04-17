namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;
    using IsThereAnyNews.Web.Interfaces.Services;

    public partial class OpmlController: Controller
    {
        private readonly IService service;

        public OpmlController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        public virtual ActionResult Import()
        {
            var viewmodel = new OpmlImporterIndexViewModel();
            return this.View("Import", viewmodel);
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Import(OpmlImporterIndexDto dto)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View("Import", null);
            }

            this.service.Import(dto);
            return this.RedirectToAction(MVC.Subscriptions.Index());
        }
    }
}