namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Infrastructure;
    using IsThereAnyNews.Web.Interfaces.Services;

    public partial class SubscriptionsController: Controller
    {
        private readonly IService service;

        public SubscriptionsController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult Index()
        {
            return this.View("Index");
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual JsonResult MyChannelList()
        {
            var viewmodel = this.service.LoadAllChannelsOfCurrentUser();
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual HttpStatusCodeResult MarkAllReadForSubscription(MarkReadForSubscriptionDto model)
        {
            this.service.MarkAllRssReadForSubscription(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual HttpStatusCodeResult MarkRssEntryViewed(long channelId)
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}