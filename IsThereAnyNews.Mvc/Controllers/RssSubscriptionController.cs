using System.Net;
using System.Web.Mvc;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssSubscriptionController : BaseController
    {
        private readonly IRssSubscriptionService rssSubscriptionService;

        public RssSubscriptionController(
            IUserAuthentication authentication, 
            ILoginService loginService, 
            IRssSubscriptionService rssSubscriptionService)
            : base(authentication, loginService)
        {
            this.rssSubscriptionService = rssSubscriptionService;
        }

        public PartialViewResult Index(long id)
        {
            var entries = this.rssSubscriptionService.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(id);
            return PartialView("_Index", entries);
        }

        public HttpStatusCodeResult MarkRssEntryViewed(long id)
        {
            this.rssSubscriptionService.MarkEntryViewed(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public HttpStatusCodeResult MarkAllReadForSubscription(MarkReadForSubscriptionDto model)
        {
            this.rssSubscriptionService.MarkAllRssReadForSubscription(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Unsubscribe(long id)
        {
            this.rssSubscriptionService.UnsubscribeCurrentUserFromChannelId(id);
            return  new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}