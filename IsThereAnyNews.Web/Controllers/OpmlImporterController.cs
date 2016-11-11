namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.ViewModels;

    [Authorize]
    public class OpmlImporterController : BaseController
    {
        private readonly IOpmlImporterService opmlImporterService;

        public OpmlImporterController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IOpmlImporterService opmlImporterService) : base(authentication, loginService)
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
            if (!this.ModelState.IsValid)
            {
                return this.View("Index", null);
            }

            this.opmlImporterService.Import(dto);
            return this.RedirectToAction("My", "RssChannel");
        }
    }
}