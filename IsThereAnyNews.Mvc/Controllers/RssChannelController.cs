using System.Web.Mvc;
using IsThereAnyNews.Mvc.Services;
using IsThereAnyNews.Mvc.Services.Implementation;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelController : Controller
    {
        private readonly IRssChannelService rssChannelsService;

        public RssChannelController() : this(new RssChannelService())
        {
        }

        public RssChannelController(IRssChannelService rssChannelsService)
        {
            this.rssChannelsService = rssChannelsService;
        }

        [HttpGet]
        public ActionResult Index(long id)
        {
            var viewmodel = this.rssChannelsService.GetViewModelFormChannelId(id);
            return this.View("Index", viewmodel);
        }
    }
}