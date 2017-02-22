using System;

namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;

    public interface IRssChannelsRepository
    {
        List<RssChannelSubscriptionWithStatisticsData> LoadAllChannelsWithStatistics();
        List<RssChannel> LoadAllChannelsForUser(long userIdToLoad);
        void SaveToDatabase(List<RssSourceWithUrlAndTitle> channelsNewToGlobalSpace);
        List<long> GetIdByChannelUrl(List<string> urlstoChannels);
        RssChannelDTO LoadRssChannel(long id);
        void UpdateRssLastUpdateTimeToDatabase(List<long> rssChannels);
        void Blah();

        List<RssChannel> LoadAllChannels();

        ChannelUrlAndTitleDTO LoadUrlAndTitle(long channelId);
        RssChannelUpdateds LoadUpdateEvents();
        RssChannelCreations LoadCreateEvents();
    }

    public class RssChannelCreatedEvent
    {
        public long Id { get; set; }
        public DateTime Updated { get; set; }
        public long ChannelId { get; set; }
        public string ChannelTitle { get; set; }
    }

    public class RssChannelUpdatedEvent
    {
        public long Id { get; set; }
        public DateTime Updated { get; set; }
        public long ChannelId { get; set; }
        public string ChannelTitle { get; set; }
    }
}