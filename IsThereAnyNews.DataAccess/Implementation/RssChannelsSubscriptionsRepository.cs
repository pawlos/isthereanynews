using System.Data.Entity.Migrations;

namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Hosting;

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
            var sqlQuery = string.Format(@"SELECT s.Title, s.Id, count(*) as Count FROM RssEntries r1
              LEFT JOIN RssEntriesToRead r2
              on r1.Id=r2.RssEntryId
              join RssChannelSubscriptions s
              on r1.RssChannelId = s.RssChannelId
              where s.UserId = {0}
              and r2.Id is null
              group by r1.RssChannelId,s.Id,s.Title
             ", currentUserId);
            var rssChannelSubscriptionDtos = this.database.Database.SqlQuery<RssChannelSubscriptionDTO>(sqlQuery).ToList();
            return rssChannelSubscriptionDtos;
        }

        public List<RssChannelSubscriptionDTO> LoadAllSubscriptionsForUser_Old(long currentUserId)
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
            var formattableString = $"UPDATE RssEntriesToRead SET IsRead=1 WHERE Id in ({string.Join(",", ids)})";
            this.database.Database.ExecuteSqlCommand(formattableString);
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