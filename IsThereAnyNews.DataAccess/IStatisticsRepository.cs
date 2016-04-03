using System.Collections.Generic;
using IsThereAnyNews.DataAccess.Implementation;

namespace IsThereAnyNews.DataAccess
{
    public interface IStatisticsRepository
    {
        List<ChannelWithSubscriptionCount> GetToReadChannels(int count);
        List<UserWithStatistics> GetUsersThatReadMostNews(int i);
        List<RssStatistics> GetNewsThatWasReadMost(int i);
    }
}