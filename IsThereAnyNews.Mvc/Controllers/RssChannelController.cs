namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Mvc.Infrastructure;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;

    [RoleAuthorize(Roles = new[] { ItanRole.User })]
    public class RssChannelController : BaseController
    {
        private readonly IRssChannelsService rssChannelsService;
        private readonly IRssSubscriptionService rssSubscriptionService;
        private readonly IUserSubscriptionService userSubscriptionServiceService;

        public RssChannelController(
            IUserAuthentication authentication,
            ILoginService loginService,
            ISessionProvider sessionProvider,
            IRssChannelsService rssChannelsService,
            IRssSubscriptionService rssSubscriptionService,
            IUserSubscriptionService userSubscriptionServiceService)
            : base(authentication, loginService, sessionProvider)
        {
            this.rssChannelsService = rssChannelsService;
            this.rssSubscriptionService = rssSubscriptionService;
            this.userSubscriptionServiceService = userSubscriptionServiceService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View("Index");
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult PublicChannels()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannels();
            return this.PartialView("_PublicChannels", viewmodel);
        }

        [HttpGet]
        public ActionResult Public(long id)
        {
            var viewmodel = this.rssChannelsService.GetViewModelFormChannelId(id);
            return this.View("Public", viewmodel);
        }

        public ActionResult My()
        {
            return this.View("My");
        }

        public JsonResult MyChannelList()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannelsOfCurrentUser();
            var listOfUsers = this.userSubscriptionServiceService.LoadAllObservableSubscription();
            viewmodel.Users = listOfUsers;
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Unsubscribe(long subscriptionId, long channelId)
        {
            this.rssSubscriptionService.UnsubscribeCurrentUserFromChannelId(subscriptionId);
            return this.RedirectToAction("Public", new { id = channelId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Subscribe(long id)
        {
            this.rssSubscriptionService.SubscribeCurrentUserToChannel(id);
            return this.RedirectToAction("Public", new { id = id });
        }


        public HttpStatusCodeResult MarkRssEntryViewed(long id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public HttpStatusCodeResult MarkAllReadForSubscription(MarkReadForSubscriptionDto model)
        {
            this.rssSubscriptionService.MarkAllRssReadForSubscription(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddChannel(AddChannelDto dto)
        {
            this.rssChannelsService.CreateNewChannelIfNotExists(dto);
            this.rssSubscriptionService.SubscribeCurrentUserToChannel(dto);
            return this.RedirectToAction("My");
        }
    }
}