using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.Mvc.Services;
using IsThereAnyNews.Mvc.Services.Implementation;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;
    using Dtos;
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
                var channelList = this.opmlImporterService.ParseToRssChannelList(dto);
                this.opmlImporterService.AddNewChannelsToGlobalSpace(channelList);
                this.opmlImporterService.AddToCurrentUserChannelList(channelList);
                return this.RedirectToAction("Index", "Syndication");
            }

            return this.View("Index", null);
        }
    }
}