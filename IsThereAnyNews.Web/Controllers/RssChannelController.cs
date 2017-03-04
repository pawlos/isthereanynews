namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Infrastructure;

    public class RssChannelController : Controller
    {
        private readonly IService service;

        public RssChannelController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View("Index");
        }

        [HttpGet]
        public ActionResult PublicChannels()
        {
            var viewmodel = this.service.LoadAllChannels();
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Public(long id)
        {
            var viewmodel = this.service.GetViewModelFormChannelId(id);
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
            var viewmodel = this.service.LoadAllChannelsOfCurrentUser();
            var listOfUsers = this.service.LoadAllObservableSubscription();
            var events = this.service.LoadEvents();
            viewmodel.Users = listOfUsers;
            viewmodel.Events = events;
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public ActionResult Unsubscribe(long channelId)
        {
            this.service.UnsubscribeCurrentUserFromChannelId(channelId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public ActionResult Subscribe(long channelId)
        {
            this.service.SubscribeCurrentUserToChannel(channelId);
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
            this.service.MarkAllRssReadForSubscription(model);
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

            this.service.CreateNewChannelIfNotExists(dto);
            this.service.SubscribeCurrentUserToChannel(dto);
            return this.RedirectToAction("My");
        }
    }
}