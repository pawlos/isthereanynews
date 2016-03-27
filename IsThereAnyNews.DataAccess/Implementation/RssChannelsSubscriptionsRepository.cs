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

        public void DeleteSubscriptionFromUser(long subscriptionId, long userId)
        {
            var channelSubscription = this.itanDatabaseContext
                .RssChannelsSubscriptions
                .Where(subscription => subscription.Id == subscriptionId)
                .Where(subscription => subscription.UserId == userId)
                .Single();
            this.itanDatabaseContext.RssChannelsSubscriptions.Remove(channelSubscription);
            this.itanDatabaseContext.SaveChanges();
        }

        public long FindSubscriptionIdOfUserAndOfChannel(long userId, long channelId)
        {
            var channelSubscription = this.itanDatabaseContext
                .RssChannelsSubscriptions
                .Where(subscription => subscription.RssChannelId == channelId)
                .Where(subscription => subscription.UserId == userId)
                .Select(subscription => subscription.Id)
                .SingleOrDefault();
            return channelSubscription;
        }

        public void CreateNewSubscriptionForUserAndChannel(long userId, long channelId)
        {
            var channelTitle = this.itanDatabaseContext.RssChannels
                .Where(channel => channel.Id == channelId)
                .Select(channel => channel.Title)
                .Single();

            var rssChannelSubscription = new RssChannelSubscription(channelId, userId, channelTitle);
            this.itanDatabaseContext.RssChannelsSubscriptions.Add(rssChannelSubscription);
            this.itanDatabaseContext.SaveChanges();
        }
    }
}