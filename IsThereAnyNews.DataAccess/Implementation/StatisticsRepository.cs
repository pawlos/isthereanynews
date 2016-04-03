using System.Collections.Generic;
using IsThereAnyNews.EntityFramework;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ItanDatabaseContext database;

        public StatisticsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<ChannelWithSubscriptionCount> GetToReadChannels(int count)
        {
            var list = this.database
                .RssChannels
                .Include(channel => channel.Subscriptions)
                .Select(ToChannelStatistics)
                .OrderByDescending(x => x.SubscriptionCount)
                .Take(count)
                .ToList();
            return list;
        }

        private ChannelWithSubscriptionCount ToChannelStatistics(RssChannel model)
        {
            var projection = new ChannelWithSubscriptionCount
            {
                Title = model.Title,
                Id = model.Id,
                SubscriptionCount = model.Subscriptions.Count
            };
            return projection;
        }

        public void GetUsersThatReadMostNews(int i)
        {
            throw new System.NotImplementedException();
        }

        public void GetNewsThatWasReadMost(int i)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ChannelWithSubscriptionCount
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public int SubscriptionCount { get; set; }
    }
}