using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace IsThereAnyNews.Services.Implementation
{
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