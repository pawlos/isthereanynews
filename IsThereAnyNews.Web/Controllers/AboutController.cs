namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    public partial class AboutController: Controller
    {
        public virtual ActionResult Index()
        {
            return this.View("Index");
        }
    }
}