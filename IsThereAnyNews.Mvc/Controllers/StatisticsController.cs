using System.Web.Mvc;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class StatisticsController : Controller
    {
        public ActionResult Index()
        {
            return this.View("Index");
        }

        public ActionResult Channels()
        {
            return this.View("Channels");
        }

        public ActionResult News()
        {
            return this.View("News");
        }

        public ActionResult Users()
        {
            return this.View("Users");
        }
    }
}