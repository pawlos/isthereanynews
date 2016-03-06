using System.ServiceModel.Channels;
using System.Web.Mvc;
using IsThereAnyNews.Mvc.Repositories;
using IsThereAnyNews.Mvc.ViewModels;

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


        public ActionResult Details(long id)
        {
            var rssChannerRepository = new RssChannelRepository();
            var channel = rssChannerRepository.Load(id);
            var viewmodel = new RssChannelsDetailsViewModel(channel);
            return this.View("Details", viewmodel);

        }
    }
}