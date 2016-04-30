using System.Web.Mvc;
using IsThereAnyNews.Services;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class StreamController : BaseController
    {
        private readonly IRssSubscriptionService rssSubscriptionService;

        public ActionResult Read(StreamType streamType, long id)
        {
            var entries = this.rssSubscriptionService.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType,id);
            return this.View("Index", entries);
        }

        public StreamController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IRssSubscriptionService rssSubscriptionService)
            : base(authentication, loginService)
        {
            this.rssSubscriptionService = rssSubscriptionService;
        }
    }
}