using System.Net;
using System.Web.Mvc;
using IsThereAnyNews.Dtos;
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

        public HttpStatusCodeResult MarkAllReadForSubscription(MarkReadForSubscriptionDto model)
        {
            this.rssSubscriptionService.MarkAllRssReadForSubscription(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}