using System.Collections.Generic;
using System.Web.Mvc;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelsController : Controller
    {
        public ActionResult Index()
        {
            var rssChannelRepository = new RssChannelRepository();
            var loadAllChannels = rssChannelRepository.LoadAllChannels();
            var viewmodel = new RssChannelsIndexViewModel(loadAllChannels);
            return this.View("Index", viewmodel);
        }
    }

    public class RssChannelsIndexViewModel
    {
        public List<RssChannel> LoadAllChannels { get; set; }

        public RssChannelsIndexViewModel(List<RssChannel> loadAllChannels)
        {
            LoadAllChannels = loadAllChannels;
        }
    }
}