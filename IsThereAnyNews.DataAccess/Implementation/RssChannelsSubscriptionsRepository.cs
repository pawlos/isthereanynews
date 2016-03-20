using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelsSubscriptionsRepository : IRssChannelsSubscriptionsRepository
    {
        private readonly ItanDatabaseContext itanDatabaseContext;

        public RssChannelsSubscriptionsRepository(ItanDatabaseContext itanDatabaseContext)
        {
            this.itanDatabaseContext = itanDatabaseContext;
        }

        public void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions)
        {
            this.itanDatabaseContext.RssChannelsSubscriptions.AddRange(rssChannelSubscriptions);
            this.itanDatabaseContext.SaveChanges();
        }

        public List<string> LoadUrlsForAllChannels()
        {
            return this.itanDatabaseContext
                .RssChannels
                .Select(channel => channel.Url)
                .ToList()
                .Select(x => x.ToLowerInvariant())
                .ToList();
        }

        public List<long> GetChannelIdSubscriptionsForUser(long currentUserId)
        {
            var rssChannelSubscriptions = this
                .itanDatabaseContext
                .RssChannelsSubscriptions
                .Where(x => x.UserId == currentUserId)
                .Select(x => x.RssChannelId)
                .ToList();
            return rssChannelSubscriptions;
        }

        public List<RssChannelSubscription> LoadAllSubscriptionsForUser(long currentUserId)
        {
            var rssChannelSubscriptions = this.itanDatabaseContext
                .RssChannelsSubscriptions
                .Where(x => x.UserId == currentUserId)
                .ToList();
            return rssChannelSubscriptions;
        }

        public List<RssChannelSubscription> LoadAllSubscriptionsWithRssEntriesToReadForUser(long currentUserId)
        {
            var rssChannelSubscriptions = this.itanDatabaseContext
                .RssChannelsSubscriptions
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.RssEntriesToRead)
                .Include(x => x.RssEntriesToRead.Select(c => c.RssEntry))
                .ToList();

            return rssChannelSubscriptions;
        }

        public bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId)
        {
             return this.itanDatabaseContext.RssChannelsSubscriptions.Any(
                x => x.UserId == currentUserId && x.Id == subscriptionId);
        }
    }
}