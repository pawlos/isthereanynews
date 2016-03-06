namespace IsThereAnyNews.Mvc.Controllers
{
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;
    using System.Xml;

    public class SyndicationController : Controller
    {
        public ActionResult Index()
        {
            string url = "http://jaroslawstadnicki.pl/feed";
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            return this.View("Index", feed);
        }
    }
}