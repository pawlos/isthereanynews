namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;

    public class UpdateService : IUpdateService
    {
        private readonly IUpdateRepository updateRepository;
        private readonly IRssEntriesRepository rssEntriesRepository;
        private readonly IRssChannelsRepository rssChannelsRepository;
        private readonly IRssChannelUpdateRepository rssChannelsUpdatedRepository;

        private readonly ISyndicationFeedAdapter syndicationFeedAdapter;

        public UpdateService(
            IUpdateRepository updateRepository,
            IRssEntriesRepository rssEntriesRepository,
            IRssChannelsRepository rssChannelsRepository,
            IRssChannelUpdateRepository rssChannelsUpdatedRepository,
            ISyndicationFeedAdapter syndicationFeedAdapter)
        {
            this.updateRepository = updateRepository;
            this.rssEntriesRepository = rssEntriesRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssChannelsUpdatedRepository = rssChannelsUpdatedRepository;
            this.syndicationFeedAdapter = syndicationFeedAdapter;
        }

        public void UpdateGlobalRss()
        {
            var rssChannels = this.updateRepository.LoadAllGlobalRssChannels();
            var rssEntriesList = new List<RssEntry>();
            foreach (var rssChannel in rssChannels)
            {
                try
                {
                    var syndicationEntries = this.syndicationFeedAdapter.Load(rssChannel.Url);

                    foreach (var item in syndicationEntries.Where(item => item.PublishDate > rssChannel.RssLastUpdatedTime))
                    {
                        var rssEntry = new RssEntry(
                            item.Id,
                            item.PublishDate,
                            item.Title,
                            item.Summary,
                            rssChannel.Id,
                            item.Url);
                        rssEntriesList.Add(rssEntry);
                    }

                    if (syndicationEntries.Any())
                    {
                        rssChannel.RssLastUpdatedTime = syndicationEntries.Max(d => d.PublishDate);
                    }

                    this.rssEntriesRepository.SaveToDatabase(rssEntriesList);

                    var rssChannelUpdated = new EventRssChannelUpdated
                    {
                        RssChannelId = rssChannel.Id
                    };

                    this.rssChannelsUpdatedRepository.SaveEvent(rssChannelUpdated);
                }
                catch (XmlException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

                rssEntriesList.Clear();
            }

            this.rssChannelsRepository.UpdateRssLastUpdateTimeToDatabase(rssChannels);
            rssChannels.Clear();
        }
    }
}