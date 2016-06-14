
namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;
    using Dtos;
    using Services;
    using ViewModels;

    [Authorize]
    public class OpmlImporterController : BaseController
    {
        private readonly IOpmlImporterService opmlImporterService;

        public OpmlImporterController(
            IUserAuthentication authentication, 
            ILoginService loginService, ISessionProvider sessionProvider,
            IOpmlImporterService opmlImporterService) : base(authentication, loginService,sessionProvider)
        {
            this.opmlImporterService = opmlImporterService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewmodel = new OpmlImporterIndexViewModel();
            return this.View("Index", viewmodel);
        }

        [HttpPost]
        public ActionResult Index(OpmlImporterIndexDto dto)
        {
            if (ModelState.IsValid)
            {
                var channelList = this.opmlImporterService.ParseToRssChannelList(dto);
                this.opmlImporterService.AddNewChannelsToGlobalSpace(channelList);
                this.opmlImporterService.AddToCurrentUserChannelList(channelList);
                return this.RedirectToAction("My", "RssChannel");
            }

            return this.View("Index", null);
        }
    }
}