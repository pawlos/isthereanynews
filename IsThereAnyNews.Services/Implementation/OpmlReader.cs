namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    public class OpmlReader : IOpmlReader
    {
        public IEnumerable<XmlNode> GetOutlines(Stream inputStream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(inputStream);
            var outlines = xmlDocument.GetElementsByTagName("outline");
            return outlines.Cast<XmlNode>();
        }
    }
}