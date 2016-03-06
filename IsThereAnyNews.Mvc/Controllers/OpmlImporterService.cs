using System.Collections.Generic;
using System.Xml;

namespace IsThereAnyNews.Mvc.Controllers
{
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
}