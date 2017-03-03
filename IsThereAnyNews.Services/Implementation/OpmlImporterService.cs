namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;

    public class OpmlImporterService : IOpmlImporterService
    {
        private readonly IUserAuthentication authentication;
        private readonly IEntityRepository entityRepository;
        private readonly IOpmlReader opmlHandler;
        public OpmlImporterService(
            IOpmlReader opmlHandler,
            IUserAuthentication authentication, 
            IEntityRepository entityRepository)
        {
            this.opmlHandler = opmlHandler;
            this.authentication = authentication;
            this.entityRepository = entityRepository;
        }

        public void AddNewChannelsToGlobalSpace(List<RssSourceWithUrlAndTitle> channelList)
        {
            var loadUrlsForAllChannels = this.entityRepository
                                             .LoadUrlsForAllChannels();
            var channelsNewToGlobalSpace =
                channelList.Where(channel => !loadUrlsForAllChannels.Contains(channel.Url.ToLowerInvariant())).ToList();
            this.entityRepository.SaveToDatabase(channelsNewToGlobalSpace);
        }

        public List<XmlNode> FilterOutInvalidOutlines(IEnumerable<XmlNode> outlines)
        {
            var validoutlines = outlines.Where(o =>
                     o.Attributes.GetNamedItem("xmlUrl") != null
                  && o.Attributes.GetNamedItem("title") != null
                  && !String.IsNullOrWhiteSpace(o.Attributes.GetNamedItem("xmlUrl").Value)
                  && !String.IsNullOrWhiteSpace(o.Attributes.GetNamedItem("title").Value));

            return validoutlines.ToList();
        }

        public void Import(OpmlImporterIndexDto dto)
        {
            var toRssChannelList = this.ParseToRssChannelList(dto);
            this.AddNewChannelsToGlobalSpace(toRssChannelList);

            var idByChannelUrl = this.entityRepository
                .GetIdByChannelUrl(toRssChannelList.Select(c => c.Url)
                    .ToList());

            var currentUserId = this.authentication.GetCurrentUserId();

            var channelsSubscribedByUser = this.entityRepository.GetChannelIdSubscriptionsForUser(currentUserId);
            channelsSubscribedByUser.ForEach(id => idByChannelUrl.Remove(id));

            idByChannelUrl.ForEach(
                c => this.entityRepository
                         .CreateNewSubscriptionForUserAndChannel(currentUserId, c));
        }

        public List<RssSourceWithUrlAndTitle> ParseToRssChannelList(OpmlImporterIndexDto dto)
        {
            var outlines = this.opmlHandler.GetOutlines(dto.ImportFile.InputStream);
            var ot = this.FilterOutInvalidOutlines(outlines);
            var urls = ot.Select(o =>
                      new RssSourceWithUrlAndTitle(
                          o.Attributes.GetNamedItem("xmlUrl").Value,
                          o.Attributes.GetNamedItem("title").Value))
                .Distinct(new RssSourceWithUrlAndTitleComparer())
                .ToList();
            return urls;
        }
    }
}