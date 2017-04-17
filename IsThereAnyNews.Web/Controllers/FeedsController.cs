namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Infrastructure;
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
            return this.View("Index");
        }

        [HttpGet]
        public virtual ActionResult Public()
        {
            var viewmodel = this.service.LoadAllChannels();
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult SubscribeToChannel(long channelId)
        {
            this.service.SubscribeCurrentUserToChannel(channelId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult UnsubscribeFromChannel(long channelId)
        {
            this.service.UnsubscribeCurrentUserFromChannelId(channelId);
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
            return this.RedirectToAction(MVC.Home.My());
        }
    }
}