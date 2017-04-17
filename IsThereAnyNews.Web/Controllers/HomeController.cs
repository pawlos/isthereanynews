namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    [AllowAnonymous]
    public partial class HomeController: Controller
    {
        public virtual ActionResult Index()
        {
            return this.View("Index");
        }

       
    }
}