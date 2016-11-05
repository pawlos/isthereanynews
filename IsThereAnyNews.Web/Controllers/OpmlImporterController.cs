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