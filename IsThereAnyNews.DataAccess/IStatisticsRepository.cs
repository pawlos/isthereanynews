using System;
using System.Collections.Generic;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.DataAccess
{
    public interface IStatisticsRepository
    {
        List<ChannelWithSubscriptionCount> GetToReadChannels(int count);
        List<UserWithStatistics> GetUsersThatReadMostNews(int i);
        List<RssStatistics> GetNewsThatWasReadMost(int i);
        List<EventRssViewed> LoadAllEventsFromAndToDate(DateTime startDate, DateTime endDate);
    }
}