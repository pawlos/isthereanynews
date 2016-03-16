using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Services.Implementation
{
    public class UpdateService : IUpdateService
    {
        private readonly IUpdateRepository updateRepository;
        private readonly IRssEntriesRepository rssEntriesRepository;
        private readonly IRssChannelRepository rssChannelsRepository;

        public UpdateService(
            IUpdateRepository updateRepository,
            IRssEntriesRepository rssEntriesRepository,
            IRssChannelRepository rssChannelsRepository)
        {
            this.updateRepository = updateRepository;
            this.rssEntriesRepository = rssEntriesRepository;
            this.rssChannelsRepository = rssChannelsRepository;
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
                }
                catch (XmlException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

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

                rssChannel.RssLastUpdatedTime = feed.Items.Max(d => d.PublishDate);
            }

            this.rssEntriesRepository.SaveToDatabase(rssEntriesList);
            this.rssChannelsRepository.UpdateRssLastUpdateTimeToDatabase(rssChannels);
        }
    }
}