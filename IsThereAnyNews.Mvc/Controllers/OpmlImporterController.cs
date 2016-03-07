using System.Threading.Tasks;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;
    using Dtos;
    using Repositories;
    using Services;
    using ViewModels;

    [Authorize]
    public class OpmlImporterController : Controller
    {
        private readonly IOpmlImporterService opmlImporterService;

        public OpmlImporterController() : this(new OpmlImporterService())
        { }

        public OpmlImporterController(IOpmlImporterService opmlImporterService)
        {
            this.opmlImporterService = opmlImporterService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var viewmodel = new OpmlImporterIndexViewModel();
            return this.View("Index", viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(OpmlImporterIndexDto dto)
        {
            if (ModelState.IsValid)
            {
                var importFromUpload = this.opmlImporterService.ImportFromUpload(dto);
                var rssChannelRepository = new RssChannelRepository();
                var imported = rssChannelRepository.AddToGlobalSpace(importFromUpload);
                this.opmlImporterService.AddToCurrentUserChannelList(imported);

                return RedirectToAction("Index", "Syndication");
            }

            return this.View("Index", null);
        }
    }
}