using System.Net;
using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    [AllowAnonymous]
    public class RssChannelController : BaseController
    {
        private readonly IRssChannelsService rssChannelsService;
        private readonly IRssSubscriptionService rssSubscriptionService;
        private readonly IRssChannelsService rssChannelsesService;

        public RssChannelController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IRssChannelsService rssChannelsService,
            IRssSubscriptionService rssSubscriptionService, 
            IRssChannelsService rssChannelsesService)
            : base(authentication, loginService)
        {
            this.rssChannelsService = rssChannelsService;
            this.rssSubscriptionService = rssSubscriptionService;
            this.rssChannelsesService = rssChannelsesService;
        }

        [HttpGet]
        public ActionResult Public(long id)
        {
            var viewmodel = this.rssChannelsService.GetViewModelFormChannelId(id);
            return this.View("Public", viewmodel);
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

        [Authorize]
        [HttpPost]
        public HttpStatusCodeResult Unsubscribe(long id)
        {
            this.rssSubscriptionService.UnsubscribeCurrentUserFromChannelId(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Subscribe(long id)
        {
            this.rssSubscriptionService.SubscribeCurrentUserToChannel(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}