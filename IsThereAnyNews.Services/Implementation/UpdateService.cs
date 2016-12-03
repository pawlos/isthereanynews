namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.ProjectionModels;

    public class UpdateService : IUpdateService
    {
        private readonly IHtmlStripper htmlStripper;
        private readonly IRssChannelsRepository rssChannelsRepository;
        private readonly IRssChannelUpdateRepository rssChannelsUpdatedRepository;
        private readonly IRssEntriesRepository rssEntriesRepository;
        private readonly ISyndicationFeedAdapter syndicationFeedAdapter;
        private readonly IUpdateRepository updateRepository;
        public UpdateService(
            IUpdateRepository updateRepository,
            IRssEntriesRepository rssEntriesRepository,
            IRssChannelsRepository rssChannelsRepository,
            IRssChannelUpdateRepository rssChannelsUpdatedRepository,
            ISyndicationFeedAdapter syndicationFeedAdapter,
            IHtmlStripper htmlStripper)
        {
            this.updateRepository = updateRepository;
            this.rssEntriesRepository = rssEntriesRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssChannelsUpdatedRepository = rssChannelsUpdatedRepository;
            this.syndicationFeedAdapter = syndicationFeedAdapter;
            this.htmlStripper = htmlStripper;
        }

        public void UpdateGlobalRss()
        {
            var rssChannels = this.updateRepository.LoadAllGlobalRssChannelsSortedByUpdate();
            var orderedEnumerable = rssChannels.OrderBy(o => o.RssLastUpdatedTime).ToList();
            foreach (var rssChannel in orderedEnumerable)
            {
                this.UpdateChannel(rssChannel);
            }

            this.rssChannelsRepository.UpdateRssLastUpdateTimeToDatabase(rssChannels.Select(x => x.Id).ToList());
            rssChannels.Clear();
        }

        private void UpdateChannel(RssChannelForUpdateDTO rssChannel)
        {
            var rssEntriesList = new List<NewRssEntryDTO>();
            try
            {
                var syndicationEntries = this.syndicationFeedAdapter.Load(rssChannel.Url);

                foreach (var item in syndicationEntries.Where(item => item.PublishDate > rssChannel.RssLastUpdatedTime))
                {
                    var rssEntry = new NewRssEntryDTO(
                                       item.Id,
                                       item.PublishDate,
                                       item.Title,
                                       item.Summary,
                                       this.htmlStripper.GetContentOnly(item.Summary),
                                       rssChannel.Id,
                                       item.Url);
                    rssEntriesList.Add(rssEntry);
                }

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