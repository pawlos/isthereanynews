namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Dtos.Feeds;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Interfaces.Services;

    public partial class FeedsController: Controller
    {
        private readonly IService service;

        public FeedsController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            var viewmodel = this.service.LoadPublicFeedsNumbers();
            return this.View("Index", viewmodel);
        }

        [HttpGet]
        public virtual JsonNetResult Public(FeedsGetPublic input)
        {
            var viewmodel = this.service.LoadPublicRssFeeds(input);
            var jsonNetResult = new JsonNetResult(viewmodel);
            return jsonNetResult;
        }

        [HttpGet]
        public virtual JsonNetResult Entries(FeedsGetEntries input)
        {
            var viewmodel = this.service.GetFeedEntries(input);
            var jsonNetResult = new JsonNetResult(viewmodel);
            return jsonNetResult;
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult SubscribeToChannel(FeedsPostSubscription input)
        {
            this.service.SubscribeCurrentUserToChannel(input);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult UnsubscribeFromChannel(FeedsPostSubscription input)
        {
            this.service.UnsubscribeCurrentUserFromChannelId(input);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult Add()
        {
            return this.View("Add");
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult Add(AddChannelDto dto)
        {
            if(this.ModelState.IsValid == false)
            {
                return this.View("Add", dto);
            }

            this.service.CreateNewChannelIfNotExists(dto);
            this.service.SubscribeCurrentUserToChannel(dto);
            return this.RedirectToAction(MVC.Subscriptions.Index());
        }

        [HttpGet]
        public virtual JsonNetResult ReadAjax(FeedsGetRead input)
        {
            var entries = this.service.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(input);
            var result = new JsonNetResult(entries);
            return result;
        }
    }

   
}