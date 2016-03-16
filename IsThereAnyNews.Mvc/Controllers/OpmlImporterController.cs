using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.Services;
using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;
    using Dtos;

    [Authorize]
    public class OpmlImporterController : Controller
    {
        private readonly IOpmlImporterService opmlImporterService;

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
                return this.RedirectToAction("My", "RssChannels");
            }

            return this.View("Index", null);
        }
    }
}