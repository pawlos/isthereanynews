using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.Services.Implementation
{
    public class UpdateService : IUpdateService
    {
        private readonly IUpdateRepository updateRepository;
        private readonly IRssEntriesRepository rssEntriesRepository;
        private readonly IRssChannelsRepository rssChannelsRepository;
        private readonly IRssChannelUpdateRepository rssChannelsUpdatedRepository;

        public UpdateService(
            IUpdateRepository updateRepository,
            IRssEntriesRepository rssEntriesRepository,
            IRssChannelsRepository rssChannelsRepository,
            IRssChannelUpdateRepository rssChannelsUpdatedRepository)
        {
            this.updateRepository = updateRepository;
            this.rssEntriesRepository = rssEntriesRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssChannelsUpdatedRepository = rssChannelsUpdatedRepository;
        }

        public void UpdateGlobalRss()
        {
            var rssChannels = this.updateRepository.LoadAllGlobalRssChannels();
            var rssEntriesList = new List<RssEntry>();
            SyndicationFeed feed = null;
            foreach (var rssChannel in rssChannels)
            {
                try
                {
                    XmlReader reader = XmlReader.Create(rssChannel.Url);
                    feed = SyndicationFeed.Load(reader);
                    reader.Close();

                    foreach (var item in feed.Items.Where(item => item.PublishDate > rssChannel.RssLastUpdatedTime))
                    {
                        var rssEntry = new RssEntry(
                            item.Id,
                            item.PublishDate,
                            item.Title?.Text ?? string.Empty,
                            item.Summary?.Text ?? string.Empty,
                            rssChannel.Id);
                        rssEntriesList.Add(rssEntry);
                    }

                    if (feed.Items.Any())
                    {
                        rssChannel.RssLastUpdatedTime = feed.Items.Max(d => d.PublishDate);
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

            rssChannels.Clear();
            this.rssChannelsRepository.UpdateRssLastUpdateTimeToDatabase(rssChannels);
        }
    }
}