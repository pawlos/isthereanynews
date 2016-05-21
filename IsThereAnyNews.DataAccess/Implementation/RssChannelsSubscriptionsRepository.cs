using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelsSubscriptionsRepository : IRssChannelsSubscriptionsRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelsSubscriptionsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions)
        {
            this.database.RssChannelsSubscriptions.AddRange(rssChannelSubscriptions);
            this.database.SaveChanges();
        }

        public List<string> LoadUrlsForAllChannels()
        {
            return this.database
                .RssChannels
                .Select(channel => channel.Url)
                .ToList()
                .Select(x => x.ToLowerInvariant())
                .ToList();
        }

        public List<long> GetChannelIdSubscriptionsForUser(long currentUserId)
        {
            var rssChannelSubscriptions = this
                .database
                .RssChannelsSubscriptions
                .Where(x => x.UserId == currentUserId)
                .Select(x => x.RssChannelId)
                .ToList();
            return rssChannelSubscriptions;
        }

        public List<RssChannelSubscription> LoadAllSubscriptionsForUser(long currentUserId)
        {
            var rssChannelSubscriptions = this.database
                .RssChannelsSubscriptions
                .Where(x => x.UserId == currentUserId)
                .ToList();
            return rssChannelSubscriptions;
        }

        public List<RssChannelSubscription> LoadAllSubscriptionsWithRssEntriesToReadForUser(long currentUserId)
        {
            var rssChannelSubscriptions = this.database
                .RssChannelsSubscriptions
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.RssEntriesToRead)
                .Include(x => x.RssEntriesToRead.Select(c => c.RssEntry))
                .ToList();

            return rssChannelSubscriptions;
        }

        public bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId)
        {
            return this.database.RssChannelsSubscriptions.Any(
               x => x.UserId == currentUserId && x.Id == subscriptionId);
        }

        public void DeleteSubscriptionFromUser(long subscriptionId, long userId)
        {
            var channelSubscription = this.database
                .RssChannelsSubscriptions
                .Where(subscription => subscription.Id == subscriptionId)
                .Where(subscription => subscription.UserId == userId)
                .Single();
            this.database.RssChannelsSubscriptions.Remove(channelSubscription);
            this.database.SaveChanges();
        }

        public long FindSubscriptionIdOfUserAndOfChannel(long userId, long channelId)
        {
            var channelSubscription = this.database
                .RssChannelsSubscriptions
                .Where(subscription => subscription.RssChannelId == channelId)
                .Where(subscription => subscription.UserId == userId)
                .Select(subscription => subscription.Id)
                .SingleOrDefault();
            return channelSubscription;
        }

        public void CreateNewSubscriptionForUserAndChannel(long userId, long channelId)
        {
            var channelTitle = this.database.RssChannels
                .Where(channel => channel.Id == channelId)
                .Select(channel => channel.Title)
                .Single();

            var rssChannelSubscription = new RssChannelSubscription(channelId, userId, channelTitle);
            this.database.RssChannelsSubscriptions.Add(rssChannelSubscription);
            this.database.SaveChanges();
        }

        public RssChannelSubscription LoadChannelInformation(long subscriptionId)
        {
            var rssChannelSubscription = this.database.RssChannelsSubscriptions
                .Include(x => x.RssChannel)
                .Single(x => x.Id == subscriptionId);

            return rssChannelSubscription;
        }

        public void MarkRead(List<long> ids)
        {
            this.database.RssEntriesToRead
                .Where(x => ids.Contains(x.Id))
                .Include(x => x.RssEntry)
                .ToList()
                .ForEach(x => x.IsRead = true);
            this.database.SaveChanges();
        }

        public bool IsUserSubscribedToChannelUrl(long currentUserId, string rssChannelLink)
        {
            var any = this.database.RssChannelsSubscriptions
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.RssChannel)
                .Where(x => x.RssChannel.Url == rssChannelLink)
                .Any();
            return any;
        }
    }
}