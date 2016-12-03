namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;

    public class UpdateService : IUpdateService
    {
        private readonly IHtmlStripper htmlStripper;
        private readonly IRssChannelsRepository rssChannelsRepository;
        private readonly IRssChannelUpdateRepository rssChannelsUpdatedRepository;
        private readonly IRssEntriesRepository rssEntriesRepository;
        private readonly ISyndicationFeedAdapter syndicationFeedAdapter;
        private readonly IUpdateRepository updateRepository;

        private readonly IMapper mapper;

        public UpdateService(
            IUpdateRepository updateRepository,
            IRssEntriesRepository rssEntriesRepository,
            IRssChannelsRepository rssChannelsRepository,
            IRssChannelUpdateRepository rssChannelsUpdatedRepository,
            ISyndicationFeedAdapter syndicationFeedAdapter,
            IHtmlStripper htmlStripper,
            IMapper mapper)
        {
            this.updateRepository = updateRepository;
            this.rssEntriesRepository = rssEntriesRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssChannelsUpdatedRepository = rssChannelsUpdatedRepository;
            this.syndicationFeedAdapter = syndicationFeedAdapter;
            this.htmlStripper = htmlStripper;
            this.mapper = mapper;
        }

        public void UpdateGlobalRss()
        {
            var rssChannels = this.updateRepository.LoadAllGlobalRssChannels();
            var orderedEnumerable = rssChannels.OrderBy(o => o.RssLastUpdatedTime);
            foreach (var rssChannel in orderedEnumerable)
            {
                this.UpdateChannel(rssChannel);
            }

            this.rssChannelsRepository
                .UpdateRssLastUpdateTimeToDatabase(rssChannels.Select(x => x.Id).ToList());
        }

        public void UpdateChannel(RssChannelForUpdateDTO rssChannel)
        {
            var rssEntriesList = new List<NewRssEntryDTO>();
            try
            {
                var syndicationEntries = this.syndicationFeedAdapter.Load(rssChannel.Url);
                var syndicationItemAdapters = syndicationEntries.Where(item => item.PublishDate > rssChannel.RssLastUpdatedTime);
                rssEntriesList = this.mapper.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(syndicationItemAdapters);
                this.rssEntriesRepository.SaveToDatabase(rssEntriesList);
            }
            catch (XmlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            this.rssChannelsUpdatedRepository.SaveEvent(rssChannel.Id);
        }
    }
}