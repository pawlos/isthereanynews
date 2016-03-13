using System.Collections.Generic;
using System.Linq;
using System.Xml;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.Mvc.Controllers;
using IsThereAnyNews.Mvc.Dtos;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class OpmlImporterService : IOpmlImporterService
    {
        private readonly IUserAuthentication userAuthentication;
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;

        public OpmlImporterService() :
            this(new UserAuthentication(),
                new RssChannelsSubscriptionsRepository())
        {
        }

        public OpmlImporterService(
            IUserAuthentication userAuthentication,
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository)
        {
            this.userAuthentication = userAuthentication;
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
        }


        public void AddToCurrentUserChannelList(List<RssChannel> importFromUpload)
        {
            var currentUserId = this.userAuthentication.GetCurrentUserId();
            var rssChannelSubscriptions = importFromUpload.Select(import => new RssChannelSubscription(import.Id, currentUserId)).ToList();
            this.rssSubscriptionsRepository.SaveToDatabase(rssChannelSubscriptions);
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