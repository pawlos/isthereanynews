using System.Collections.Generic;
using IsThereAnyNews.DataAccess.Implementation;

namespace IsThereAnyNews.DataAccess
{
    public interface IStatisticsRepository
    {
        List<ChannelWithSubscriptionCount> GetToReadChannels(int count);
        void GetUsersThatReadMostNews(int i);
        void GetNewsThatWasReadMost(int i);
    }
}