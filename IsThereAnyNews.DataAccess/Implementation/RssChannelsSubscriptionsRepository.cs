using System.Data.Entity.Migrations;

namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper.QueryableExtensions;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public class RssChannelsSubscriptionsRepository : IRssChannelsSubscriptionsRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelsSubscriptionsRepository(ItanDatabaseContext database)
        {
            this.database = database;
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

        public void DeleteSubscriptionFromUser(long channelId, long userId)
        {
            var channelSubscription = this.database
                                          .RssChannelsSubscriptions
                                          .Where(subscription => subscription.RssChannelId == channelId)
                                          .Where(subscription => subscription.UserId == userId)
                                          .Single();
            this.database.RssChannelsSubscriptions.Remove(channelSubscription);
            this.database.SaveChanges();
        }

        public bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId)
        {
            return this.database.RssChannelsSubscriptions.Any(
               x => x.UserId == currentUserId && x.Id == subscriptionId);
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

        public bool IsUserSubscribedToChannelId(long currentUserId, long channelId)
        {
            var any = this.database.RssChannelsSubscriptions.Any(x => x.UserId == currentUserId && x.RssChannelId == channelId);
            return any;
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

        public List<RssChannelSubscriptionDTO> LoadAllSubscriptionsForUser(long currentUserId)
        {
            var channelSubscriptions = from subs in this.database.RssChannelsSubscriptions
                                       join rs in this.database.RssEntriesToRead on subs.Id equals rs.RssChannelSubscriptionId into rss
                                       join channel in this.database.RssChannels on subs.RssChannelId equals channel.Id into channels
                                       where subs.UserId == currentUserId
                                       select new RssChannelSubscriptionDTO
                                       {
                                           Id = subs.Id,
                                           Count = rss.Count(r => r.IsRead == false),
                                           Title = channels.FirstOrDefault().Title,
                                           RssChannelId = subs.RssChannelId,
                                           ChannelUrl = channels.FirstOrDefault().Url
                                       };

            return channelSubscriptions.ToList();
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

        public RssChannelInformationDTO LoadChannelInformation(long subscriptionId)
        {
            var rssChannelSubscription = this.database.RssChannelsSubscriptions
                .Include(x => x.RssChannel)
                .Where(x => x.Id == subscriptionId)
                .ProjectTo<RssChannelInformationDTO>()
                .Single();

            return rssChannelSubscription;
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

        public void MarkRead(List<long> ids)
        {
            var rssEntryToReads = ids.Select(i => new RssEntryToRead { Id = i, IsRead = true }).ToList();
            rssEntryToReads.ForEach(
                r =>
                    {
                        database.RssEntriesToRead.Attach(r);
                        var entry = database.Entry(r);
                        entry.Property(p => p.IsRead).IsModified = true;
                    });

            this.database.Configuration.ValidateOnSaveEnabled = false;
            this.database.SaveChanges();
            this.database.Configuration.ValidateOnSaveEnabled = true;
            rssEntryToReads.ForEach(x => this.database.Entry(x).State = EntityState.Detached);
        }

        public void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions)
        {
            this.database.RssChannelsSubscriptions.AddRange(rssChannelSubscriptions);
            this.database.SaveChanges();
        }
        public void Subscribe(long idByChannelUrl, long currentUserId)
        {
            var title = this.database.RssChannels.Single(x => x.Id == idByChannelUrl).Title;
            var rssChannelSubscription = new RssChannelSubscription(idByChannelUrl, currentUserId, title);
            this.database.RssChannelsSubscriptions.Add(rssChannelSubscription);
            this.database.SaveChanges();
        }

        public void Subscribe(long idByChannelUrl, long currentUserId, string channelIdRssChannelName)
        {
            var rssChannelSubscription = new RssChannelSubscription(idByChannelUrl, currentUserId, channelIdRssChannelName);
            this.database.RssChannelsSubscriptions.Add(rssChannelSubscription);
            this.database.SaveChanges();
        }
    }
}