namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Infrastructure;

    public class RssChannelController : BaseController
    {
        private readonly IRssChannelsService rssChannelsService;
        private readonly IRssSubscriptionService rssSubscriptionService;
        private readonly IUserSubscriptionService userSubscriptionServiceService;
        private readonly ISystemSubscriptionService systemSubscriptionService;

        public RssChannelController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IRssChannelsService rssChannelsService,
            IRssSubscriptionService rssSubscriptionService,
            IUserSubscriptionService userSubscriptionServiceService,
            ISystemSubscriptionService systemSubscriptionService)
            : base(authentication, loginService)
        {
            this.rssChannelsService = rssChannelsService;
            this.rssSubscriptionService = rssSubscriptionService;
            this.userSubscriptionServiceService = userSubscriptionServiceService;
            this.systemSubscriptionService = systemSubscriptionService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View("Index");
        }

        [HttpGet]
        public ActionResult PublicChannels()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannels();
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Public(long id)
        {
            var viewmodel = this.rssChannelsService.GetViewModelFormChannelId(id);
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public ActionResult My()
        {
            return this.View("My");
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public JsonResult MyChannelList()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannelsOfCurrentUser();
            var listOfUsers = this.userSubscriptionServiceService.LoadAllObservableSubscription();
            var events = this.systemSubscriptionService.LoadEvents();
            viewmodel.Users = listOfUsers;
            viewmodel.ChannelEvents = events;
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public ActionResult Unsubscribe(long channelId)
        {
            this.rssSubscriptionService.UnsubscribeCurrentUserFromChannelId(channelId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public ActionResult Subscribe(long channelId)
        {
            this.rssSubscriptionService.SubscribeCurrentUserToChannel(channelId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public HttpStatusCodeResult MarkRssEntryViewed(long channelId)
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public HttpStatusCodeResult MarkAllReadForSubscription(MarkReadForSubscriptionDto model)
        {
            this.rssSubscriptionService.MarkAllRssReadForSubscription(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public ActionResult Add()
        {
            return this.View("Add");
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public ActionResult Add(AddChannelDto dto)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View("Add", dto);
            }

            this.rssChannelsService.CreateNewChannelIfNotExists(dto);
            this.rssSubscriptionService.SubscribeCurrentUserToChannel(dto);
            return this.RedirectToAction("My");
        }
    }
}