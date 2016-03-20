using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class UpdateGlobalController : BaseController
    {
        private readonly IUpdateService updateService;

        public UpdateGlobalController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IUpdateService updateService) : base(authentication, loginService)
        {
            this.updateService = updateService;
        }

        public ActionResult Index()
        {
            this.updateService.UpdateGlobalRss();
            return View("Index");
        }
    }
}