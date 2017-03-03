namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Exceptionless;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;

    public class UpdateService : IUpdateService
    {
        private readonly ISyndicationFeedAdapter syndicationFeedAdapter;
        private readonly IMapper mapper;
        private readonly IEntityRepository entityRepository;

        public UpdateService(
            ISyndicationFeedAdapter syndicationFeedAdapter,
            IMapper mapper, IEntityRepository entityRepository)
        {
            this.syndicationFeedAdapter = syndicationFeedAdapter;
            this.mapper = mapper;
            this.entityRepository = entityRepository;
        }

        public void UpdateGlobalRss()
        {
            try
            {
                var rssChannel = this.entityRepository.LoadChannelToUpdate();
                var lastUpdate = this.entityRepository.GetLatestUpdateDate(rssChannel.Id);
                this.entityRepository.SaveEvent(rssChannel.Id);
                this.UpdateChannel(rssChannel, lastUpdate);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void UpdateChannel(RssChannelForUpdateDTO rssChannel, DateTime lastUpdate)
        {
            var syndicationEntries = this.syndicationFeedAdapter.Load(rssChannel.Url);
            var syndicationItemAdapters = syndicationEntries.Where(item => item.PublishDate > lastUpdate);
            var rssEntriesList = this.mapper.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(syndicationItemAdapters);
            rssEntriesList.ForEach(r => r.RssChannelId = rssChannel.Id);
            this.entityRepository.SaveToDatabase(rssEntriesList);
            this.entityRepository.SaveEvent(rssChannel.Id);
        }
    }
}