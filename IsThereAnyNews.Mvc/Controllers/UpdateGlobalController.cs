using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class UpdateGlobalController : Controller
    {
        private readonly IUpdateService updateService;

        public UpdateGlobalController(IUpdateService updateService)
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