using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly IRssChannelsRepository rssRepository;

        public ActionResult Index()
        {
            return this.View("Index");
        }

        public HomeController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IRssChannelsRepository rssRepository)
            : base(authentication, loginService)
        {
            this.rssRepository = rssRepository;
        }

        public ActionResult Blah()
        {
            rssRepository.Blah();
            return this.RedirectToAction("Index");
        }
    }
}