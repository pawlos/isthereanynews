namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            return this.View("Index");
        }
    }
}