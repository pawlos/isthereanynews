using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssSubscriptionController : Controller
    {
        private readonly IRssSubscriptionService rssSubscriptionService;

        public RssSubscriptionController(IRssSubscriptionService rssSubscriptionService)
        {
            this.rssSubscriptionService = rssSubscriptionService;
        }

        public PartialViewResult Index(long id)
        {
            var entries = this.rssSubscriptionService.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(id);
            return PartialView("_Index", entries);
        }
    }
}