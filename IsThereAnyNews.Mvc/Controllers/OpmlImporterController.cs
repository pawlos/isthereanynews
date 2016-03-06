namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    public class OpmlImporterController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewmodel = new OpmlImporterIndexViewModel();
            return this.View("Index", viewmodel);
        }

        [HttpPost]
        public ActionResult Index(OpmlImporterIndexDto dto)
        {
            if (ModelState.IsValid)
            {
                OpmlImporterService service = new OpmlImporterService();
                var importFromUpload = service.ImportFromUpload(dto);
                RssChannelRepository rssChannelRepository = new RssChannelRepository();
                rssChannelRepository.AddToGlobalSpace(importFromUpload);

                return RedirectToAction("Index", "Syndication");
            }
            return this.View("Index", null);
        }
    }
}