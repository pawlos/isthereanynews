using System.Collections.Generic;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssChannelsRepository
    {
        List<RssChannelSubscriptionWithStatisticsData> LoadAllChannelsWithStatistics();
        RssChannel Load(long id);
        List<RssChannel> LoadAllChannelsForUser(long userIdToLoad);
        void SaveToDatabase(List<RssChannel> channelsNewToGlobalSpace);
        List<long> GetIdByChannelUrl(List<string> urlstoChannels);
        RssChannel LoadRssChannel(long id);
        void UpdateRssLastUpdateTimeToDatabase(List<RssChannel> rssChannels);
        void Blah();

        List<RssChannel> LoadAllChannels();
    }
}