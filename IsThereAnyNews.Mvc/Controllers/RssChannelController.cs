using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelController : Controller
    {
        private readonly IRssChannelService rssChannelsService;

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