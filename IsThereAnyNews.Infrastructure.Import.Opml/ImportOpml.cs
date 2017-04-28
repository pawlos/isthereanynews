using System.Collections.Generic;
using System.Linq;

namespace IsThereAnyNews.Infrastructure.Import.Opml
{
    using System.IO;
    using System.Xml;
    using IsThereAnyNews.ProjectionModels.Mess;

    public class ImportOpml : IImportOpml
    {
        public IEnumerable<XmlNode> GetOutlines(Stream inputStream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(inputStream);
            var outlines = xmlDocument.GetElementsByTagName("outline");
            return outlines.Cast<XmlNode>();
        }

        public List<XmlNode> FilterOutInvalidOutlines(IEnumerable<XmlNode> outlines)
        {
            var validoutlines = outlines.Where(
                    o => o.Attributes.GetNamedItem("xmlUrl") != null
                         && o.Attributes.GetNamedItem("title") != null
                         && !string.IsNullOrWhiteSpace(o.Attributes.GetNamedItem("xmlUrl")
                                                        .Value)
                         && !string.IsNullOrWhiteSpace(o.Attributes.GetNamedItem("title")
                                                        .Value));
            return validoutlines.ToList();
        }

        public List<RssSourceWithUrlAndTitle> SelectUrls(List<XmlNode> ot)
        {
            var urls = ot.Select(
                        o => new RssSourceWithUrlAndTitle(
                                o.Attributes.GetNamedItem("xmlUrl")
                                 .Value,
                                o.Attributes.GetNamedItem("title")
                                 .Value))
                        .Distinct(new RssSourceWithUrlAndTitleComparer())
                        .ToList();
            return urls;
        }
    }

    public interface IImportOpml
    {
        IEnumerable<XmlNode> GetOutlines(Stream inputStream);
        List<XmlNode> FilterOutInvalidOutlines(IEnumerable<XmlNode> outlines);
        List<RssSourceWithUrlAndTitle> SelectUrls(List<XmlNode> ot);
    }
}
