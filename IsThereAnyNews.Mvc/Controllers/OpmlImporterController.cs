using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    public class OpmlImporterController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var viewmodel = new OpmlImporterIndexViewModel();
            return this.View("Index", viewmodel);
        }

        [HttpPost]
        public ActionResult Index(OpmlImporterIndexDto dto)
        {
            if (ModelState.IsValid)
            {
                OpmlImporterService service = new OpmlImporterService();
                var importFromUpload = service.ImportFromUpload(dto);
                RssChannelRepository rssChannelRepository = new RssChannelRepository();
                rssChannelRepository.AddToGlobalSpace(importFromUpload);

                return RedirectToAction("Index", "Syndication");
            }
            return this.View("Index", null);
        }
    }

    public class RssChannelRepository
    {
        static private List<RssChannel> listOfChannels = new List<RssChannel>();

        public void AddToGlobalSpace(List<RssChannel> importFromUpload)
        {
            listOfChannels.AddRange(importFromUpload);
        }

        public List<RssChannel> LoadAllChannels()
        {
            return listOfChannels;
        }
    }

    public class OpmlImporterService
    {
        public List<RssChannel> ImportFromUpload(OpmlImporterIndexDto dto)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(dto.ImportFile.InputStream);
            var outlines = xmlDocument.GetElementsByTagName("outline");
            var urls = new List<RssChannel>();
            foreach (XmlNode outline in outlines)
            {
                var itemUrl = outline.Attributes.GetNamedItem("xmlUrl");
                var itemTitle = outline.Attributes.GetNamedItem("title");
                if (itemUrl != null)
                {
                    urls.Add(new RssChannel(itemUrl.Value, itemTitle.Value));
                }
            }

            return urls;
        }
    }

    public class OpmlImporterIndexDto
    {
        public HttpPostedFileBase ImportFile { get; set; }
    }

    public class OpmlImporterIndexViewModel
    {
    }

    public class RssChannel
    {
        public RssChannel(string url, string title)
        {
            Url = url;
            Title = title;
        }

        public string Title { get; set; }
        public string Url { get; set; }
    }
}