namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View("Index");
        }
    }
}