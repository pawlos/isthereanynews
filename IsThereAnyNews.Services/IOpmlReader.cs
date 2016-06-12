using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace IsThereAnyNews.Services
{
    public interface IOpmlReader
    {
        IEnumerable<XmlNode> GetOutlines(Stream inputStream);
    }
}