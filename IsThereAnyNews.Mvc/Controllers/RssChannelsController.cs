using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    [Authorize]
    public class RssChannelsController : BaseController
    {
        private readonly IRssChannelsService rssChannelsesService;

        public RssChannelsController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IRssChannelsService rssChannelsesService) : base(authentication, loginService)
        {
            this.rssChannelsesService = rssChannelsesService;
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
    }
}