namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    public interface IOpmlReader
    {
        IEnumerable<XmlNode> GetOutlines(Stream inputStream);
    }
}