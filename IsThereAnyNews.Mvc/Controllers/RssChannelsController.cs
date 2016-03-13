using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelsController : Controller
    {
        private readonly IRssChannelsService rssChannelsesService;

        public RssChannelsController(
            IRssChannelsService rssChannelsesService)
        {
            this.rssChannelsesService = rssChannelsesService;
        }

        public ActionResult Index()
        {
            var viewmodel = this.rssChannelsesService.LoadAllChannels();
            return this.View("Index", viewmodel);
        }

        [Authorize]
        public ActionResult My()
        {
            var viewmodel = this.rssChannelsesService.LoadAllChannelsOfCurrentUser();
            return this.View("My", viewmodel);
        }
    }
}