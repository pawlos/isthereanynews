namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using AutoMapper;

    using Exceptionless;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;

    public class UpdateService : IUpdateService
    {
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
            IMapper mapper)
        {
            this.updateRepository = updateRepository;
            this.rssEntriesRepository = rssEntriesRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssChannelsUpdatedRepository = rssChannelsUpdatedRepository;
            this.syndicationFeedAdapter = syndicationFeedAdapter;
            this.mapper = mapper;
        }

        public void UpdateGlobalRss()
        {
            try
            {
                var rssChannel = this.updateRepository.LoadChannelToUpdate();
                this.rssChannelsUpdatedRepository.SaveEvent(rssChannel.Id);
                this.UpdateChannel(rssChannel);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void UpdateChannel(RssChannelForUpdateDTO rssChannel)
        {

            var syndicationEntries = this.syndicationFeedAdapter.Load(rssChannel.Url);
            var syndicationItemAdapters = syndicationEntries.Where(item => item.PublishDate > rssChannel.RssLastUpdatedTime);
            var rssEntriesList = this.mapper.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(syndicationItemAdapters);
            rssEntriesList.ForEach(r => r.RssChannelId = rssChannel.Id);
            this.rssEntriesRepository.SaveToDatabase(rssEntriesList);
            this.rssChannelsUpdatedRepository.SaveEvent(rssChannel.Id);

        }
    }
}