using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    [AllowAnonymous]
    public class RssChannelController : BaseController
    {
        private readonly IRssChannelService rssChannelsService;

        public RssChannelController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IRssChannelService rssChannelsService) : base(authentication, loginService)
        {
            this.rssChannelsService = rssChannelsService;
        }

        [HttpGet]
        public ActionResult Index(long id)
        {
            var viewmodel = this.rssChannelsService.GetViewModelFormChannelId(id);
            return this.View("Index", viewmodel);
        }
    }
}