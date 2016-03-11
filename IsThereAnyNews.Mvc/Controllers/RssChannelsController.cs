using System.Web.Mvc;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelsController : Controller
    {
        private readonly IRssChannelService rssChannelsService;

        public RssChannelsController() : this(
            new RssChannelsService())
        {
        }

        private RssChannelsController(
            IRssChannelService rssChannelsService)
        {
            this.rssChannelsService = rssChannelsService;
        }

        public ActionResult Index()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannels();
            return this.View("Index", viewmodel);
        }

        public ActionResult Details(long id)
        {
            var viewmodel = this.rssChannelsService.Load(id);
            return this.View("Details", viewmodel);
        }

        [Authorize]
        public ActionResult My()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannelsOfCurrentUser();
            return this.View("My", viewmodel);
        }
    }
}