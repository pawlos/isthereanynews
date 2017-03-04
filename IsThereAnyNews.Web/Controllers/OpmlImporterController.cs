namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.ViewModels;

    [Authorize]
    public class OpmlImporterController : Controller
    {
        private readonly IService service;

        public OpmlImporterController(
            IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewmodel = new OpmlImporterIndexViewModel();
            return this.View("Index", viewmodel);
        }

        [HttpPost]
        public ActionResult Index(OpmlImporterIndexDto dto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Index", null);
            }

            this.service.Import(dto);
            return this.RedirectToAction("My", "RssChannel");
        }
    }
}