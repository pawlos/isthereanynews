namespace IsThereAnyNews.Mvc.Services
{
    using System.Collections.Generic;
    using System.Xml;
    using Controllers;
    using Dtos;
    using Models;

    public class OpmlImporterService : IOpmlImporterService
    {
        private readonly IUserAuthentication userAuthentication;
        private readonly IUserRepository usersRepository;

        public OpmlImporterService() :
            this(new UserAuthentication(),
                new UserRepository())
        {
        }

        public OpmlImporterService(
            IUserAuthentication userAuthentication,
            IUserRepository usersRepository)
        {
            this.userAuthentication = userAuthentication;
            this.usersRepository = usersRepository;
        }


        public void AddToCurrentUserChannelList(List<RssChannel> importFromUpload)
        {
            var currentUserId = this.userAuthentication.GetCurrentUserId();
            this.usersRepository.AddToUserRssList(currentUserId, importFromUpload);
        }

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