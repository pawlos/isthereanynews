namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Services;

    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly IRssChannelsRepository rssRepository;

        public HomeController(
            IUserAuthentication authentication,
            ILoginService loginService,
            ISessionProvider sessionProvider,
            IRssChannelsRepository rssRepository)
            : base(authentication, loginService, sessionProvider)
        {
            this.rssRepository = rssRepository;
        }

        public ActionResult Index()
        {
            return this.View("Index");
        }

        public ActionResult Blah()
        {
            rssRepository.Blah();
            return this.RedirectToAction("Index");
        }
    }
}