using System.Collections.Generic;
using System.Linq;
using System.Xml;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Services.Implementation
{
    public class OpmlImporterService : IOpmlImporterService
    {
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;
        private readonly ISessionProvider sessionProvider;
        private readonly IRssChannelsRepository rssChannelses;

        public OpmlImporterService(
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            ISessionProvider sessionProvider,
            IRssChannelsRepository rssChannelses)
        {
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.sessionProvider = sessionProvider;
            this.rssChannelses = rssChannelses;
        }


        public void AddToCurrentUserChannelList(List<RssChannel> importFromUpload)
        {
            var urlstoChannels = importFromUpload.Select(x => x.Url.ToLowerInvariant()).ToList();
            var listOfChannelsIds = this.rssChannelses.GetIdByChannelUrl(urlstoChannels);
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            var existringChannelIdSubscriptions = this.rssSubscriptionsRepository.GetChannelIdSubstrictionsForUser(currentUserId);
            var rssChannelSubscriptions = listOfChannelsIds.Select(import => new RssChannelSubscription(import, currentUserId, importFromUpload.Single(x=>x.Id == import).Title)).ToList();
            var subscriptionsToSave =
                rssChannelSubscriptions.Where(newSub => !existringChannelIdSubscriptions.Contains(newSub.RssChannelId))
                    .ToList();
            this.rssSubscriptionsRepository.SaveToDatabase(subscriptionsToSave);
        }

        public List<RssChannel> ParseToRssChannelList(OpmlImporterIndexDto dto)
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

        public void AddNewChannelsToGlobalSpace(List<RssChannel> channelList)
        {
            List<string> loadUrlsForAllChannels = this.rssSubscriptionsRepository.LoadUrlsForAllChannels();
            var channelsNewToGlobalSpace = channelList.Where(channel => !loadUrlsForAllChannels.Contains(channel.Url.ToLowerInvariant())).ToList();
            this.rssChannelses.SaveToDatabase(channelsNewToGlobalSpace);
        }
    }
}